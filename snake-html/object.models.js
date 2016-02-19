/// <reference path="bower/lib/lodash/dist/lodash.min.js" />
/// <reference path="bower/lib/jquery/dist/jquery.min.js" />

var pojos = {
    position: function (initialData) {
        this.X = 0;
        this.Y = 0;

        _.assign(this, initialData);
    },

    grid: function (initialData) {
        this.color = 'black';
        this.position = new pojos.position({ X: 15, Y: 15});
        this.cellHeight = 10;
        this.cellWidth = 10;
        this.cells = 10;

        _.assign(this, initialData);
    },
    snake: function (initialData) {
        this.stepH = 0;
        this.stepV = 0;
        this.trail = [new pojos.position({ X: 2, Y: 2 })];// [[0,0]]

        _.assign(this, initialData);
    },
    fruit: function (initialData) {
        this.color = 'blue';
        this.position = new pojos.position({ X: 5, Y: 5 });

        _.assign(this, initialData);
    },
    game: function (initialData) {
        this.canvas = null;
        this.canvasSelector = "canvas"; //$("canvas").get(0).getContext("2d");
        this.ctx = null;

        this.fruit = new pojos.fruit();
        this.grid = new pojos.grid();
        this.snake = new pojos.snake();
        this.score = 0;

        this.isRunning = false;

        this.threadStepGame = null;
        this.threadRenderGame = null;

        _.assign(this, initialData);

        

    }

};

pojos.game.prototype.startGame = function () {
    this.isRunning = true;
    this.canvas = $(this.canvasSelector);
    this.ctx = this.canvas.get(0).getContext("2d");
    var snake = this.snake;

    $("body").on("keydown", function (e) {
        switch (e.keyCode) {
            case 40: //Keys.Down:
                snake.stepV = 1; snake.stepH = 0;
                break;
            case 38: //Keys.Up:
                snake.stepV = -1; snake.stepH = 0;
                break;
            case 39: //Keys.Right:
                snake.stepV = 0; snake.stepH = 1;
                break;
            case 37: //Keys.Left:
                snake.stepV = 0; snake.stepH = -1;
                break;
            default:
                break;
        }
    });


    this.snake.trail = [new pojos.position({ X: 2, Y: 2 })];

    this.threadStepGame = setInterval(this.stepGame, 100, this);
    this.threadRenderGame = setInterval(this.renderGame, 90, this);
};

pojos.game.prototype.stopGame = function () {

    this.isRunning = false;

    clearInterval(this.threadStepGame);

};

function rndInt(min, max) {
    return Math.round((Math.random() * max) + min);
}
pojos.game.prototype.stepGame = function (game) {
    var grid = game.grid;
    var fruit = game.fruit;
    var snake = game.snake;
    var snakeHead = game.snake.trail[0];

    console.log(fruit.position);
    console.log(snakeHead);

    if (fruit.position.X == snakeHead.X + snake.stepH && fruit.position.Y == snakeHead.Y + snake.stepV) {
        snake.trail.push(new pojos.position({ X: fruit[0] + snake.stepH, Y: fruit[1] + snake.stepV }));
        game.score += 1;
        if (snake.trail.length >= grid.cells * grid.cells) {
            gameOver();
        }
        while (true) {
            fruit.position = new pojos.position({ X: rndInt(0, grid.cells), Y: rndInt(0, grid.cells) });
            var isFreeCell = true;

            _.each(snake.trail, function (trail) {
                if (fruit.position.X != trail.X || fruit.position.Y != trail.Y) return true;

                isFreeCell = false;
                return false;
            });

            if (isFreeCell) break;
        }

        return;
    } 

    snake.trail[snake.trail.length - 1].X = snake.trail[0].X + snake.stepH;
    snake.trail[snake.trail.length - 1].Y = snake.trail[0].Y + snake.stepV;

    //snake.tail[0][0] = snake.tail[0][0] + snake.stepH;
    //snake.tail[0][1] = snake.tail[0][1] + snake.stepV;

    var last = snake.trail.splice(snake.trail.length - 1, 1);
    
    snake.trail = last.concat(snake.trail);

};

pojos.game.prototype.renderGame = function (game) {
    var grid = game.grid;
    var ctx = game.ctx;
    var canvas = game.canvas;

    ctx.clearRect(0, 0, canvas.width(), canvas.height());
    ctx.fillStyle = 'black';

    for (var i = 0; i < grid.cells; i++) {
        ctx.moveTo((i * grid.cellWidth) + grid.position.X, grid.position.Y);
        ctx.lineTo((i * grid.cellWidth) + grid.position.X, grid.cells * grid.cellWidth);

        ctx.moveTo(grid.position.Y, (i * grid.cellHeight) + grid.position.Y);
        ctx.lineTo(grid.cells * grid.cellHeight, (i * grid.cellHeight) + grid.position.Y);

    }

    _.each(game.snake.trail, function (trail) {
        ctx.fillRect(
            (trail.X * grid.cellWidth) + grid.position.X, 
            (trail.Y * grid.cellHeight) + grid.position.Y, 
            grid.cellWidth, grid.cellHeight
        );
    });

    ctx.fillStyle = game.fruit.color;
    ctx.fillRect(
        (game.fruit.position.X * grid.cellWidth) + grid.position.X,
        (game.fruit.position.Y * grid.cellHeight) + grid.position.X,
        grid.cellWidth, grid.cellHeight
    );

    ctx.stroke();

    //g.FillRectangle(grid.fruitPen.Brush, new Rectangle(fruit[0] * 10 + grid.position.X, fruit[1] * 10 + grid.position.Y, 10, 10));

    //foreach (var tail in snake.tail) {
    //    g.FillRectangle(grid.pen.Brush, new Rectangle(tail[0] * 10 + grid.position.X, tail[1] * 10 + grid.position.Y, 10, 10));
    //}

    //g.DrawString(game.score.ToString(), f.Font, grid.pen.Brush, new PointF((float)(grid.cells * grid.cellH + 50), 50F));


};
