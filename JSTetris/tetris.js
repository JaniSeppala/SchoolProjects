let currentTetromino = Math.floor(Math.random() * 7); //Numero jolla määritetään minkä tyyppinen palikka on pelialueella tällä hetkellä
let nextTetromino = Math.floor(Math.random() * 7); //Numero jolla määritetään minkä tyyppinen palikka pelataan seuraavaksi
let tetrominoRotation = 0; //Luku jolla pidetään kirjaa siitä missä asennossa pelattava palikka on
let gameOver = false; //Tämä kertoo milloin pelaaja on hävinnyt pelin
let autoDescentHandle;//Tänne tallennetaan intervallifunktio joka siirtää tetrominon automaattisesti alaspäin.
let autoDescentInterval = 793;//Tänne tallennetaan tieto tetrominon tippumisnopeudesta millisekuntteina. Mitä isompi luku sitä hitaampi tiputusnopeus
let softDrop = false;//Tämä ilmoittaa painaako käyttäjä drop-nappia.
let softDropHandle;////Tänne tallennetaan intervallifunktio joka siirtää tetrominon alaspäin kun painetaan drop-nappia.
let highestRow = 19; //Tänne tallennetaan korkein rivi pelialueella
let score = 0;//Tämä pitää kirjaa pelinaikana saaduista pisteistä
let level = 0;//Tämä kertoo millä tasolla ollaan menossa
let lines = 0;//Tämä kertoo montako riviä on saatu tuhottua
let highscore = localStorage.getItem('jstetris-highscore');//Tällä pidetään kirjaa siitä mikä on pelaajan piste-ennätys. Tämä arvo tallennetaan local storageen aina kun tulee uusi ennätys

const scoreForTetromino = 10;//Montako pistettä saa per tetromino
const scoreForRow = 100;//Montako pistettä saa kun tuhoaa yhden rivin
const playingFieldColor = 'gray';//Pelialueen taustaväri
const playingField = Array(20); //Taulukko joka pitää kirjaa siitä mitä palikoita pelialueella on
const tetromino = Array(4); //Tetromino sisältää neljä block-oliota.


//Tarkistetaan onko local storagessa pelaajan ennätys ja jos on niin näytetään se päävalikossa.
if (!isNull(highscore && !isNaN(highscore))) {
    document.getElementById('highscore').innerHTML='HIGHSCORE:<br>' + highscore;
} else {
    highscore = 0;
}


//Tällä funktiolla piilotetaan game over valikko ja näytetään päävalikko. Ei palauta mitään
function mainMenu() {
    document.getElementById('highscore').innerHTML='HIGHSCORE:<br>' + highscore;
    document.getElementById('game-over-menu').style.display='none';
    document.getElementById('start-menu').style.display='initial';
}

//Tällä funktiolla aloitetaan peli ja piilotetaan aloitusvalikko. Ei palauta mitään.
function startGame() {
    gameOver = false;
    let levelSelect = document.getElementById('level')
    level = levelSelect.options[levelSelect.selectedIndex].value;
    document.getElementById('stats').innerHTML=`SCORE:<br>${score}<br>LEVEL: ${level}<br>LINES: ${lines}`;
    autoDescentInterval = 793 - (77 * level);
    initializePlayingField();
    createTetromino();
    document.addEventListener("keydown", inputListener);
    autoDescentHandle = setInterval(autoDescent, autoDescentInterval);
    softDropHandle = setInterval(()=>{
        if (softDrop) {
            moveTetromino(0,1);
        }
    }, 70);
    document.getElementById('start-menu').style.display='none';
}

function gameOverFunc() {
    clearInterval(autoDescentHandle);
    clearInterval(softDropHandle);
    document.removeEventListener("keydown", inputListener);
    if (score > highscore) {
        localStorage.setItem('jstetris-highscore', score);
        highscore =score;
        document.getElementById('new-highscore').style.display='initial';
    } else {
        document.getElementById('new-highscore').style.display='none';
    }
    document.getElementById('your-score').innerHTML = 'Your score: ' + score;
    document.getElementById('game-over-menu').style.display='initial';
}

function inputListener(event) {
    event.preventDefault();
    switch (event.key) {
        case "Up" :
        case "ArrowUp" :
            rotateTetromino();
            break; 
        case "Down" :
        case "ArrowDown" : 
            moveTetromino(0,1);
            break;        
        case "Left" :
        case "ArrowLeft" : 
            moveTetromino(-1,0);
            break;
        case "Right" :
        case "ArrowRight" :
            moveTetromino(1,0);
            break;
    }
}

//Tätä funktiota kutsutaan kun palikkaa käännetään. Ei palauta mitään.
function rotateTetromino() {
    switch (currentTetromino) {
        case 0 : // I
            switch (tetrominoRotation) {
                case 0 :
                    if (!checkCollision(tetromino[0].x + 2, tetromino[0].y - 2) && !checkCollision(tetromino[1].x + 1, tetromino[1].y - 1) && !checkCollision(tetromino[3].x - 1, tetromino[3].y + 1)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[1].x,tetromino[1].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x += 2;
                        tetromino[0].y -= 2;
                        tetromino[1].x++;
                        tetromino[1].y--;
                        tetromino[3].x--;
                        tetromino[3].y++;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[1].x,tetromino[1].y, tetromino[1].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 1 :
                    if (!checkCollision(tetromino[0].x - 2, tetromino[0].y + 2) && !checkCollision(tetromino[1].x - 1, tetromino[1].y + 1) && !checkCollision(tetromino[3].x + 1, tetromino[3].y - 1)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[1].x,tetromino[1].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x -= 2;
                        tetromino[0].y += 2;
                        tetromino[1].x--;
                        tetromino[1].y++;
                        tetromino[3].x++;
                        tetromino[3].y--;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[1].x,tetromino[1].y, tetromino[1].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation--;
                    }
                    break;
            }
            break;
        case 2 : // T
            switch (tetrominoRotation) {
                case 0 :
                    if (!checkCollision(tetromino[2].x - 1, tetromino[2].y - 1)) {
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        tetromino[2].x--;
                        tetromino[2].y--;
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        tetrominoRotation++;
                    }
                    break;
                case 1 :
                    if (!checkCollision(tetromino[3].x + 1, tetromino[3].y - 1)) {
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[3].x++;
                        tetromino[3].y--;
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 2 :
                    if (!checkCollision(tetromino[0].x + 1, tetromino[0].y + 1)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        tetromino[0].x++;
                        tetromino[0].y++;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        tetrominoRotation++;
                    }
                    break;
                case 3 :
                    if (!checkCollision(tetromino[0].x - 1, tetromino[0].y - 1) && !checkCollision(tetromino[2].x + 1, tetromino[2].y + 1) && !checkCollision(tetromino[3].x - 1, tetromino[3].y + 1)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x--;
                        tetromino[0].y--;
                        tetromino[2].x++;
                        tetromino[2].y++;
                        tetromino[3].x--;
                        tetromino[3].y++;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation = 0;
                    }
                    break;
            }
            break;
        case 3 :  // S
            switch (tetrominoRotation) {
                case 0 :
                    if (!checkCollision(tetromino[2].x + 1, tetromino[2].y - 2) && !checkCollision(tetromino[3].x + 1, tetromino[3].y)) {
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[2].x++;
                        tetromino[2].y -= 2;
                        tetromino[3].x++;
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 1 :
                    if (!checkCollision(tetromino[2].x - 1, tetromino[2].y + 2) && !checkCollision(tetromino[3].x - 1, tetromino[3].y)) {
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[2].x--;
                        tetromino[2].y += 2;
                        tetromino[3].x--;
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation--;
                    }  
                    break;
            }
            break;
        case 4 :  // Z
            switch (tetrominoRotation) {
                case 0 :
                    if (!checkCollision(tetromino[0].x + 2, tetromino[0].y - 1) && !checkCollision(tetromino[3].x, tetromino[3].y - 1)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x += 2;
                        tetromino[0].y--;
                        tetromino[3].y--;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 1 :
                    if (!checkCollision(tetromino[0].x - 2, tetromino[0].y + 1) && !checkCollision(tetromino[3].x, tetromino[3].y + 1)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x -= 2;
                        tetromino[0].y++;
                        tetromino[3].y++;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation--;
                    }
                    break;
            }
            break;
        case 5 :  // J
            switch (tetrominoRotation) {
                case 0 :
                    if (!checkCollision(tetromino[0].x + 1, tetromino[0].y - 1) && !checkCollision(tetromino[2].x - 1, tetromino[2].y + 1) && !checkCollision(tetromino[3].x - 2, tetromino[3].y)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x++;
                        tetromino[0].y--;
                        tetromino[2].x--;
                        tetromino[2].y++;
                        tetromino[3].x -= 2;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 1 :
                    if (!checkCollision(tetromino[0].x + 1, tetromino[0].y + 1) && !checkCollision(tetromino[2].x - 1, tetromino[2].y - 1) && !checkCollision(tetromino[3].x, tetromino[3].y - 2)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x++;
                        tetromino[0].y++;
                        tetromino[2].x--;
                        tetromino[2].y--;
                        tetromino[3].y -= 2;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 2 :
                    if (!checkCollision(tetromino[0].x - 1, tetromino[0].y + 1) && !checkCollision(tetromino[2].x + 1, tetromino[2].y - 1) && !checkCollision(tetromino[3].x + 2, tetromino[3].y)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x--;
                        tetromino[0].y++;
                        tetromino[2].x++;
                        tetromino[2].y--;
                        tetromino[3].x += 2;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 3 :
                    if (!checkCollision(tetromino[0].x - 1, tetromino[0].y - 1) && !checkCollision(tetromino[2].x + 1, tetromino[2].y + 1) && !checkCollision(tetromino[3].x, tetromino[3].y + 2)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x--;
                        tetromino[0].y--;
                        tetromino[2].x++;
                        tetromino[2].y++;
                        tetromino[3].y += 2;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation = 0;
                    }
                    break;
            }
            break;
        case 6 :  // L
            switch (tetrominoRotation) {
                case 0 :
                    if (!checkCollision(tetromino[0].x + 1, tetromino[0].y - 1) && !checkCollision(tetromino[2].x - 1, tetromino[2].y + 1) && !checkCollision(tetromino[3].x, tetromino[3].y - 2)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x++;
                        tetromino[0].y--;
                        tetromino[2].x--;
                        tetromino[2].y++;
                        tetromino[3].y -= 2;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 1 :
                    if (!checkCollision(tetromino[0].x + 1, tetromino[0].y + 1) && !checkCollision(tetromino[2].x - 1, tetromino[2].y - 1) && !checkCollision(tetromino[3].x + 2, tetromino[3].y)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x++;
                        tetromino[0].y++;
                        tetromino[2].x--;
                        tetromino[2].y--;
                        tetromino[3].x += 2;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 2 :
                    if (!checkCollision(tetromino[0].x - 1, tetromino[0].y + 1) && !checkCollision(tetromino[2].x + 1, tetromino[2].y - 1) && !checkCollision(tetromino[3].x, tetromino[3].y + 2)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x--;
                        tetromino[0].y++;
                        tetromino[2].x++;
                        tetromino[2].y--;
                        tetromino[3].y += 2;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation++;
                    }
                    break;
                case 3 :
                    if (!checkCollision(tetromino[0].x - 1, tetromino[0].y - 1) && !checkCollision(tetromino[2].x + 1, tetromino[2].y + 1) && !checkCollision(tetromino[3].x - 2, tetromino[3].y)) {
                        drawBlock(tetromino[0].x,tetromino[0].y, playingFieldColor);
                        drawBlock(tetromino[2].x,tetromino[2].y, playingFieldColor);
                        drawBlock(tetromino[3].x,tetromino[3].y, playingFieldColor);
                        tetromino[0].x--;
                        tetromino[0].y--;
                        tetromino[2].x++;
                        tetromino[2].y++;
                        tetromino[3].x -= 2;
                        drawBlock(tetromino[0].x,tetromino[0].y, tetromino[0].color);
                        drawBlock(tetromino[2].x,tetromino[2].y, tetromino[2].color);
                        drawBlock(tetromino[3].x,tetromino[3].y, tetromino[3].color);
                        tetrominoRotation = 0;
                    }
                    break;
            }
            break;
    }
}

//Tällä alustetaan pelialueen ruudut ja niitä vastaava globaalin playingField-taulukko. Ei palauta mitään.
function initializePlayingField() {
    const cells = document.getElementsByTagName('td');
    for (let i = 0; i < cells.length; i++) {
        cells[i].style.backgroundColor=playingFieldColor;
    }
    playingField.fill('');
    for (let i = 0; i < playingField.length; i++) {
        playingField[i] = Array(10).fill('');
    }
}

//Tällä liikutetaan palikkaa automaattisesti alaspäin
function autoDescent() {
    if(!gameOver) {
        moveTetromino(0,1);
    } else {
        gameOverFunc();
    }
}

//Tällä liikutetaan tetrispalikkaa pelialueella. Ei palauta mitään.
function moveTetromino(xAmount, yAmount) {
    if (!tetromino.some(element=>{return checkCollision(element.x + xAmount, element.y + yAmount);})) {
        tetromino.forEach(element => {
            drawBlock(element.x, element.y, playingFieldColor);
            element.x += xAmount;
            element.y += yAmount;
        });
        tetromino.forEach(element => {
            drawBlock(element.x, element.y, element.color);
        });
    } else if (yAmount > 0) {
        if (tetromino.some(element => {return element.y < 0;})) {
            gameOver = true;
        } else {
            tetromino.forEach(element => {
                if (element.y < highestRow) {
                    highestRow = element.y
                }
                playingField[element.y][element.x] = element;
            });
            const lowest = lowestBlock();
            const highest = highestBlock();
            let rows = 0;//Pitää kirjaa montako riviä saatiin tiputettua kerralla
            createTetromino();
            for (let row = lowest; row >= highest; row--) {
                if (checkRow(row)) {
                    for (let i = row; i >= highestRow; i--) {
                        dropRow(i);
                    }
                    rows++;
                    highestRow++;
                    row++;
                }
            }
            if (rows === 0) {
                score += scoreForTetromino * (level + 1);
            } else {
                score += scoreForRow * rows * (level + 1);
                lines += rows;
                if (lines >= (level + 1) * 10) {
                    level++;
                    clearInterval(autoDescentHandle);
                    if (level < 10) {
                        autoDescentInterval -= 77;
                    } else if (autoDescentInterval > 30){
                        autoDescentInterval -= 10;
                    }
                    autoDescentHandle = setInterval(autoDescent, autoDescentInterval);
                }
            }
            document.getElementById('stats').innerHTML = `SCORE:<br>${score}<br>LEVEL: ${level}<br>LINES: ${lines}`;
        }
    }
}

//Tällä metodilla tarkastetaan osuuko palikka siirrettäessä johonkin toiseen palikkaan pelialueella. Jos osuu niin metodi palauttaa arvon true ja jos ei niin palautusarvo on false.
function checkCollision(xCoord, yCoord) {
    if (yCoord < 0) {
        return false;
    }
    if (xCoord < 0 || xCoord > 9 || yCoord > 19 || typeof playingField[yCoord][xCoord] === 'object') {
        return true;
    } else {
        return false;
    }
}

//Tällä luodaan yksittäinen palikka. Palautusarvo on palikkaa kuvaava olio.
function createBlock(blockColor, xCoord, yCoord) {
    return {
        //Yksittäisen palikan olio. Jokainen Tetromino koostuu neljästä tällaisesta.
        color: blockColor, //Palikan väri. Tämän perusteella piirretään väri pelialueelle.
        x: xCoord, //Palikan x-koordinaatti pelialueella.
        y: yCoord //Palikan y-koordinaatti pelialueella.
    }
}

//Tällä luodaan uusi Tetromino joka koostuu neljästä palikka-oliosta jotka sijoitetaan automaattisesti globaaliin tetromino-taulukkoon. 
function createTetromino() {
    switch (nextTetromino) {
        case 0: { // I
            tetromino[0] = createBlock('cyan', 3, -1);
            tetromino[1] = createBlock('cyan', 4, -1);
            tetromino[2] = createBlock('cyan', 5, -1);
            tetromino[3] = createBlock('cyan', 6, -1);
            break;
        }
        case 1: { // O
            tetromino[0] = createBlock('yellow', 4, -2);
            tetromino[1] = createBlock('yellow', 5, -2);
            tetromino[2] = createBlock('yellow', 4, -1);
            tetromino[3] = createBlock('yellow', 5, -1);
            break;
        }
        case 2: { // T
            tetromino[0] = createBlock('purple', 3, -2);
            tetromino[1] = createBlock('purple', 4, -2);
            tetromino[2] = createBlock('purple', 5, -2);
            tetromino[3] = createBlock('purple', 4, -1);
            break;
        }
        case 3: { // S
            tetromino[0] = createBlock('green', 4, -2);
            tetromino[1] = createBlock('green', 5, -2);
            tetromino[2] = createBlock('green', 3, -1);
            tetromino[3] = createBlock('green', 4, -1);
            break;
        }
        case 4: { // Z
            tetromino[0] = createBlock('red', 3, -2);
            tetromino[1] = createBlock('red', 4, -2);
            tetromino[2] = createBlock('red', 4, -1);
            tetromino[3] = createBlock('red', 5, -1);
            break;
        }
        case 5: { // J
            tetromino[0] = createBlock('blue', 3, -2);
            tetromino[1] = createBlock('blue', 4, -2);
            tetromino[2] = createBlock('blue', 5, -2);
            tetromino[3] = createBlock('blue', 5, -1);
            break;
        }
        case 6: { // L
            tetromino[0] = createBlock('orange', 3, -2);
            tetromino[1] = createBlock('orange', 4, -2);
            tetromino[2] = createBlock('orange', 5, -2);
            tetromino[3] = createBlock('orange', 3, -1);
            break;
        }
    }

    currentTetromino = nextTetromino;
    tetrominoRotation = 0;
    nextTetromino = Math.floor(Math.random() * 7);
    drawNextTetromino();
}

function drawNextTetromino() {
    
    //Ensin tyhjennetään vanha palikka näytöstä
    for (let row = 0; row < 2; row++) {
        for (let column = 0; column < 4; column++) {
            document.getElementById(`n${row}${column}`).style.backgroundColor=playingFieldColor;
        }      
    }

    //Ja sitten piirretään uusi palikka
    switch (nextTetromino) {
        case 0: { // I
            document.getElementById(`n00`).style.backgroundColor='cyan';
            document.getElementById(`n01`).style.backgroundColor='cyan';
            document.getElementById(`n02`).style.backgroundColor='cyan';
            document.getElementById(`n03`).style.backgroundColor='cyan';
            break;
        }
        case 1: { // O
            document.getElementById(`n01`).style.backgroundColor='yellow';
            document.getElementById(`n02`).style.backgroundColor='yellow';
            document.getElementById(`n11`).style.backgroundColor='yellow';
            document.getElementById(`n12`).style.backgroundColor='yellow';
            break;
        }
        case 2: { // T
            document.getElementById(`n00`).style.backgroundColor='purple';
            document.getElementById(`n01`).style.backgroundColor='purple';
            document.getElementById(`n02`).style.backgroundColor='purple';
            document.getElementById(`n11`).style.backgroundColor='purple';
            break;
        }
        case 3: { // S
            document.getElementById(`n01`).style.backgroundColor='green';
            document.getElementById(`n02`).style.backgroundColor='green';
            document.getElementById(`n10`).style.backgroundColor='green';
            document.getElementById(`n11`).style.backgroundColor='green';
            break;
        }
        case 4: { // Z
            document.getElementById(`n00`).style.backgroundColor='red';
            document.getElementById(`n01`).style.backgroundColor='red';
            document.getElementById(`n11`).style.backgroundColor='red';
            document.getElementById(`n12`).style.backgroundColor='red';
            break;
        }
        case 5: { // J
            document.getElementById(`n00`).style.backgroundColor='blue';
            document.getElementById(`n01`).style.backgroundColor='blue';
            document.getElementById(`n02`).style.backgroundColor='blue';
            document.getElementById(`n12`).style.backgroundColor='blue';
            break;
        }
        case 6: { // L
            document.getElementById(`n00`).style.backgroundColor='orange';
            document.getElementById(`n01`).style.backgroundColor='orange';
            document.getElementById(`n02`).style.backgroundColor='orange';
            document.getElementById(`n10`).style.backgroundColor='orange';
            break;
        }
    }
}

//Tällä määritetään painaako pelaaja drop-nappulaa
function enableSoftDrop(value) {
    if (value) {
        softDrop = true;
    } else {
        softDrop = false;
    }
}

//Tällä piirretään tetrominon palikka pelialueelle
function drawBlock(xCoord, yCoord, color) {
    if (xCoord >= 0 && xCoord < 10 && yCoord >= 0 && yCoord < 20) {
        document.getElementById(`${yCoord}${xCoord}`).style.backgroundColor = color;
    }
}

//Tällä tarkistetaan onko rivi täysi. Jos on niin palauttaa true ja jos ei niin false.
function checkRow(rowIndex) {
    let counter = 0;
    playingField[rowIndex].forEach(element => {
        if (typeof element === 'object') {
            counter++;
        }
    });
    if (counter === playingField[rowIndex].length) {
        return true;
    } else {
        return false;
    }
}

function highestBlock() {//Palauttaa tetrominon korkeimman y-koordinaatin
    let highest = tetromino[0].y;
    tetromino.forEach(element => {
        if (element.y < highest) {
            highest = element.y;
        }
    });
    return highest;
}

function lowestBlock() {//Palauttaa tetrominon alimman y-koordinaatin
    let lowest = tetromino[0].y;
    tetromino.forEach(element => {
        if (element.y > lowest) {
            lowest = element.y;
        }
    });
    return lowest;
}

//Tällä poistetaan rivi eli se tyhjentää rivin ja kopio ylemmän rivin tiedot siihen tilalle ja piirtää muutokset pelialueelle
function dropRow(rowIndex) {
    for (let i = 0; i < playingField[rowIndex].length; i++) {
        if (rowIndex > 0) {
            playingField[rowIndex][i] = playingField[rowIndex - 1][i];
            if (typeof playingField[rowIndex][i] === 'object') {
                drawBlock(i, rowIndex, playingField[rowIndex][i].color);
            }
            else {
                drawBlock(i, rowIndex, playingFieldColor);
            }
        } else {
            playingField[rowIndex][i] = '';
            drawBlock(i, rowIndex, playingFieldColor);
        }
    }
}