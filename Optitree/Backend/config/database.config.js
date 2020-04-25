const mysql = require('mysql');

// Configuring the database

const dbConfig = {
    host: 'localhost',//Add your database url here
    user: 'MyDatabaseUser', //Add your database username here
    password: 'IDDQD', //Add your database users password here
    database: 'optitree' //Add your database name here
}

// Connecting to the database
const connection = mysql.createConnection(dbConfig);

connection.connect((error) => {
    if (error) {
        return console.log(error.message);
    }
    console.log('Succesfully connected to the database!');
});

module.exports = connection;

