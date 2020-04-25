const dbConnection = require('../config/database.config');
const authenticator = require('./authentication.controller');

// Create and Save a new User
exports.create = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    if (!req.body) {
        return res.status(400).send({message: 'The request body cannot be empty!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        if (!result.payload.administrator) {
            return res.status(403).send({message: 'Only administrators can add users!'});
        }
        const user = req.body;
        const values = [user.username, user.pswd, user.firstname, user.lastname];
        if (user.teacher === true && user.administrator === true) {
            values.push(true, true);
        }
        else if (user.teacher === true) {
            values.push(true, false);
        }
        else if (user.administrator === true) {
            values.push(false, true);
        } else {
            values.push(false, false);        
        }
        dbConnection.query('INSERT INTO users(username, pswd, firstname, lastname, teacher, administrator) VALUES (?, ?, ?, ?, ?, ?)', values, (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while adding the user.', error: error});
            }
            res.send(result);
        });
    });
};

// Retrieve and return all users from the database.
exports.findAll = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        dbConnection.query('SELECT userID, username, firstname, lastname, teacher, administrator FROM users', (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the users.'});
            }
            res.send(result);
        });
    });
};

// Find a single user with a username
exports.findOne = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        dbConnection.query('SELECT userID, username, firstname, lastname, teacher, administrator FROM users WHERE username = ?', [req.params.username], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the user.'});
            }
            res.send(result);
        });
    });
};

exports.findStudents = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        dbConnection.query('SELECT userID, username, firstname, lastname, teacher, administrator FROM users WHERE teacher = false AND administrator = false', (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the teachers.'});
            }
            res.send(result);
        });
    });
};

exports.findTeachers = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        dbConnection.query('SELECT userID, username, firstname, lastname, teacher, administrator FROM users WHERE teacher = true', (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the teachers.'});
            }
            res.send(result);
        });
    });
};

// Update a user identified by the username in the request
exports.update = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    if (!req.body) {
        return res.status(400).send({message: 'The request body cannot be empty!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        const user = req.body;
        if (!result.payload.administrator && user.username !== result.payload.username) {
            return res.status(403).send({message: 'Only administrators can update other users information!'});
        }
        const values = [user.username, user.pswd, user.firstname, user.lastname];
        if (user.teacher === true && user.administrator === true) {
            values.push(true, true);
        }
        else if (user.teacher === true) {
            values.push(true, false);
        }
        else if (user.administrator === true) {
            values.push(false, true);
        } else {
            values.push(false, false);        
        }
        values.push(req.params.username);
        dbConnection.query('UPDATE users SET username = ?, pswd = ?, firstname = ?, lastname = ?, teacher = ?, administrator = ? WHERE username = ?', values, (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while updating the user.', error: error});
            }
            res.send(result);
        });
    });
};

// Delete a user with the specified username in the request
exports.delete = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    if (!req.body) {
        return res.status(400).send({message: 'The request body cannot be empty!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        if (!result.payload.administrator) {
            return res.status(403).send({message: 'Only administrators can delete users!'});
        }
        dbConnection.query('DELETE FROM users WHERE username = ?', [req.params.username], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while deleting the user.', error: error});
            }
            res.send(result);
        });
    });
};

exports.findUsersWorkspaces = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        dbConnection.query('SELECT workspaces.workspaceID, workspacename FROM workspaces INNER JOIN workspaceusers ON workspaces.workspaceID = workspaceusers.workspaceID WHERE workspaceusers.userID = ?', [result.payload.userID], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the users workspaces.', error: error});
            }
            res.send(result);
        });
    });
};