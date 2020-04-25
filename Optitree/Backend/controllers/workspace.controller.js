const dbConnection = require('../config/database.config');
const authenticator = require('./authentication.controller');

// Create and Save a new Workspace
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
        if (!result.payload.administrator && !result.payload.teacher) {
            return res.status(403).send({message: 'Only administrators and teachers can add workspaces!'});
        }
        const ws = req.body;
        const values = [ws.workspacename, result.payload.username];
        dbConnection.query('INSERT INTO workspaces(workspacename, creatorName) VALUES (?, ?)', values, (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while adding the workspace.', error: error});
            }
            res.send(result);
        });
    });
};

// Retrieve and return all workspaces from the database.
exports.findAll = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        if (result.payload.administrator) {
            dbConnection.query('SELECT * FROM workspaces', (error, result)=>{
                if (error) {
                    return res.status(500).send({message: 'An error occured while fetching the workspaces.'});
                }
                return res.send(result);
            });
        }
        if (result.payload.teacher) {
            const values = [result.payload.userID, result.payload.username];
            dbConnection.query('SELECT Workspaces.workspaceID, workspacename, creatorname FROM Workspaces LEFT OUTER JOIN WorkspaceUsers ON Workspaces.workspaceID = WorkspaceUsers.workspaceID WHERE (WorkspaceUsers.userID = ? AND WorkspaceUsers.teacher = true) OR Workspaces.creatorname = ? GROUP BY Workspaces.workspaceID', values, (error, result)=>{
                if (error) {
                    return res.status(500).send({message: 'An error occured while fetching the workspaces.', error: error});
                }
                return res.send(result);
            });
        }
    });
};

// Find a single workspace with a workspace name
exports.findOne = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        dbConnection.query('SELECT * FROM workspaces WHERE workspacename = ?', [req.params.workspacename], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the workspace.'});
            }
            res.send(result);
        });
    });
};

exports.findWorkspaceTeachers = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    if (!req.body.wsID) {
        return res.status(400).send({message: 'No wsID found in the request body!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        dbConnection.query('SELECT * FROM WorkspaceUsers WHERE workspaceID = ? AND teacher = true', [req.body.wsID], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the workspace teachers.'});
            }
            res.send(result);
        });
    });
};

// Update a workspace identified by the workspace name in the request
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
        const ws = req.body;
        if (!result.payload.administrator && !result.payload.teacher) {
            return res.status(403).send({message: 'Only administrators and teachers can edit workspaces!'});
        }
        const values = [ws.wsname, req.params.wsname];
        values.push(req.params.username);
        dbConnection.query('UPDATE workspaces SET workspacename = ? WHERE workspacename = ?', values, (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while updating the workspace.', error: error});
            }
            res.send(result);
        });
    });
};

// Delete a workspace with the specified workspace name in the request
exports.delete = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        if (!result.payload.administrator && !result.payload.teacher) {
            return res.status(403).send({message: 'Only administrators and teachers can delete workspaces!'});
        }
        dbConnection.query('DELETE FROM workspaces WHERE workspacename = ?', [req.params.workspacename], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while deleting the user.', error: error});
            }
            res.send(result);
        });
    });
};

exports.addStudentsToWorkspace = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        if (!result.payload.administrator && !result.payload.teacher) {
            return res.status(403).send({message: 'Only administrators and teachers can add students to workspaces!'});
        }
        if (!req.body || req.body.userArray.length < 1) {
            return res.status(400).send({message: 'No userArray found in the request body!'});
        }
        if (!req.body.wsID) {
            return res.status(400).send({message: 'No workspaceID(wsID) found in the request body!'});
        }
        const wsID = req.body.wsID;
        dbConnection.beginTransaction((err) => {
            if (err) {
                return res.status(500).send({message: 'An error occured while adding the users to the workspace.', error: err});
            }
            req.body.userArray.forEach(userID => {
                dbConnection.query('INSERT INTO WorkspaceUsers(workspaceID, userID) VALUES (?, ?)', [wsID, userID], (error, result)=>{
                    if (error) {
                        return dbConnection.rollback(() => {
                            throw error;
                        });
                    }
                }); 
            });
            dbConnection.commit((err) => {
                if (err) {
                    return dbConnection.rollback(() => {
                        throw err;
                    });
                }
                res.send(result);
            });
        });
    });
}

exports.addTeachersToWorkspace = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        if (!result.payload.administrator && !result.payload.teacher) {
            return res.status(403).send({message: 'Only administrators and teachers can add teachers to workspaces!'});
        }
        if (!req.body || req.body.userArray.length < 1) {
            return res.status(400).send({message: 'No userArray found in the request body!'});
        }
        if (!req.body.wsID) {
            return res.status(400).send({message: 'No workspaceID(wsID) found in the request body!'});
        }
        const wsID = req.body.wsID;
        dbConnection.beginTransaction((err) => {
            if (err) {
                return res.status(500).send({message: 'An error occured while adding the users to the workspace.', error: err});
            }
            req.body.userArray.forEach(userID => {
                dbConnection.query('INSERT INTO WorkspaceUsers(workspaceID, userID, teacher) VALUES (?, ?, true)', [wsID, userID], (error, result)=>{
                    if (error) {
                        return dbConnection.rollback(() => {
                            throw error;
                        });
                    }
                }); 
            });
            dbConnection.commit((err) => {
                if (err) {
                    return dbConnection.rollback(() => {
                        throw err;
                    });
                }
                res.send(result);
            });
        });
    });
}

exports.removeUsersFromWorkspace = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        if (!result.payload.administrator && !result.payload.teacher) {
            return res.status(403).send({message: 'Only administrators and teachers can remove students from workspaces!'});
        }
        if (!req.body || req.body.userArray.length < 1) {
            return res.status(400).send({message: 'No userArray found in the request body!'});
        }
        if (!req.body.wsID) {
            return res.status(400).send({message: 'No workspaceID(wsID) found in the request body!'});
        }
        const wsID = req.body.wsID;
        dbConnection.beginTransaction((err) => {
            if (err) {
                return res.status(500).send({message: 'An error occured while adding the users to the workspace.', error: err});
            }
            req.body.userArray.forEach(userID => {
                dbConnection.query('DELETE FROM WorkspaceUsers WHERE workspaceID = ? AND userID = ?', [wsID, userID], (error, result)=>{
                    if (error) {
                        return dbConnection.rollback(() => {
                            throw error;
                        });
                    }
                }); 
            });
            dbConnection.commit((err) => {
                if (err) {
                    return dbConnection.rollback(() => {
                        throw err;
                    });
                }
                res.send(result);
            });
        });
    });
}

exports.findWorkspaceUsers = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        dbConnection.query('SELECT * FROM WorkspaceUsers WHERE workspaceID = (SELECT workspaceID FROM workspaces WHERE workspacename = ?) AND teacher = false', [req.params.workspacename], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the workspace users.', error:error});
            }
            res.send(result);
        });
    });
};