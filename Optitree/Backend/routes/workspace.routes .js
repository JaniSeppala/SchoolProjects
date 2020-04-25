const express = require('express')
const router = express.Router();
const workspaces = require('../controllers/workspace.controller');

//WORKSPACE ROUTES

//Root
router.get('/', (req, res) => {
    res.json({"message": "This is the root of the workspaces route."});
});

// Create a new Workspace
router.post('/addWorkspace', workspaces.create);

//Add users as a student to a Workspace
router.post('/addUsers', workspaces.addStudentsToWorkspace);

//Add teachers to a Workspace
router.post('/addTeachers', workspaces.addTeachersToWorkspace);

//Remove students from a Workspace
router.post('/removeUsers', workspaces.removeUsersFromWorkspace);

// Retrieve all Workspaces
router.get('/allWorkspaces', workspaces.findAll);

// Retrieve a single Workspace with workspace name
router.get('/:workspacename', workspaces.findOne);

//Retrieve all the teachers of a single workspace
router.post('/findTeachers', workspaces.findWorkspaceTeachers);

//Retrieve all the users of a single workspace
router.get('/findUsers/:workspacename', workspaces.findWorkspaceUsers);

// Update a Workspace with Workspace name
router.put('/:workspacename', workspaces.update);

// Delete a Workspace with Workspace name
router.delete('/:workspacename', workspaces.delete);

module.exports = router;