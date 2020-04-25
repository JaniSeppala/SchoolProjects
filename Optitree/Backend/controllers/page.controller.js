const dbConnection = require('../config/database.config');
const authenticator = require('./authentication.controller');

// Create and Save a new Page
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
            return res.status(403).send({message: 'Only administrators and teachers can add pages!'});
        }
        const page = req.body;
        const values = [page.pageName, page.pageHTML, page.workspaceID, page.visible];
        dbConnection.query('INSERT INTO pages(pageName, pageHTML, workspaceID, visible) VALUES (?, ?, ?, ?)', values, (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while adding the page.', error: error});
            }
            res.send(result);
        });
    });
};

// Find all pages of a single workspace with a workspaceID
exports.findAllWorkspacePages = (req, res) => {
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
        dbConnection.query('SELECT pageID, pageName, workspaceID, visible FROM pages WHERE workspaceID = ?', [req.body.wsID], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the pages.'});
            }
            res.send(result);
        });
    });
};

//Find a single page with a pageID
exports.findOne = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    if (!req.body.pageID) {
        return res.status(400).send({message: 'No pageID found in the request body!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        dbConnection.query('SELECT * FROM pages WHERE pageID = ?', [req.body.pageID], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while fetching the pages.'});
            }
            res.send(result);
        });
    });
};


// Update a page identified by the pageID in the request body
exports.update = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    if (!req.body.pageID) {
        return res.status(400).send({message: 'The pageID was not found in the request body!'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        if (!result.payload.administrator && !result.payload.teacher) {
            return res.status(403).send({message: 'Only administrators and teachers can update the pages!'});
        }
        const page = req.body;
        const values = [page.pageName, page.pageHTML, page.visible, page.pageID];
        dbConnection.query('UPDATE pages SET pageName = ?, pageHTML = ?, visible = ? WHERE pageID = ?', values, (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while updating the page.', error: error});
            }
            res.send(result);
        });
    });
};

// Delete a user with the specified pageID in the request body
exports.delete = (req, res) => {
    if (!req.headers['x-access-token']) {
        return res.status(400).send({message: 'No access token found in the header!'});
    }
    if (!req.body.pageID) {
        return res.status(400).send({message: 'The pageID was not found in the request body'});
    }
    authenticator.verifyToken(req.headers['x-access-token'], (result)=> {
        if (!result.verified) {
            return res.status(400).send({message: 'Invalid access token!'});
        }
        if (!result.payload.administrator && !result.payload.teacher) {
            return res.status(403).send({message: 'Only administrators and teachers can delete pages!'});
        }
        dbConnection.query('DELETE FROM pages WHERE pageID = ?', [req.body.pageID], (error, result)=>{
            if (error) {
                return res.status(500).send({message: 'An error occured while deleting the page.', error: error});
            }
            res.send(result);
        });
    });
};