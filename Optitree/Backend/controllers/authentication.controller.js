const jwt = require('jsonwebtoken'); // moduli jolla luodaan JWT
const dbConnection = require('../config/database.config');

const secret = 'Optitree';//Koodi jota käytetään tokenin salauksen purkamiseen

exports.authenticateUser = (req, res) => {
    dbConnection.query('SELECT * FROM users WHERE username = ? AND pswd = ?', [req.body.username, req.body.pswd], (error, result)=>{
        if (error) {
            return res.status(500).send({message: 'There was an error while connecting to the database! Please try again later.'});
        }
        if (result.length < 1) {
            return res.send({message: 'Username or password incorrect!'});
        }
        const payload = {
            userID: result[0].userID,
            username: result[0].username,
            firstname: result[0].firstname,
            lastname: result[0].lastname,
            teacher: result[0].teacher,
            administrator: result[0].administrator
        }
        const token = jwt.sign(payload, secret, {
            expiresIn: 60 * 60 * 24 // expires in 24 hours
        });
        return res.send({
            message: 'Kirjauduttiin onnistuneesti. Tämän olion ominaisuus "token" sisältää JWT-tokenin, jota tarvitset API:n käyttämiseen. Kirjautuminen on voimassa 24h.',
            token: token
        });
    });
};

exports.verifyToken = (token, callback) => {
    let object;
    jwt.verify(token, secret, function(err, decodedtoken) {
        if (err) {
            object = {verified: false, message: 'Token ei ollut validi!'};
        } else {
            const decoded = jwt.decode(token);
            object = {verified: true, message: 'Token oli kelvollinen!', payload: decoded};
        }
    });
    callback(object);
}