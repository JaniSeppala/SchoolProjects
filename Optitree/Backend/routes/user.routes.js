const express = require('express')
const router = express.Router();
const users = require('../controllers/user.controller');

//USER ROUTES

//Root
router.get('/', (req, res) => {
    res.json({"message": "This is the root of the users route."});
});

// Create a new User
router.post('/addUser', users.create);

// Retrieve users workspaces
router.get('/findWorkspaces', users.findUsersWorkspaces);

// Retrieve all Users
router.get('/allUsers', users.findAll);

//Retrieve all students
router.get('/allStudents', users.findStudents);

//Retrieve all teachers
router.get('/allTeachers', users.findTeachers);

// Retrieve a single user with userId
router.get('/:username', users.findOne);

// Update a User with userId
router.put('/:username', users.update);

// Delete a User with userId
router.delete('/:username', users.delete);

module.exports = router;