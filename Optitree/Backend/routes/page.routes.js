const express = require('express')
const router = express.Router();
const pages = require('../controllers/page.controller');

//PAGE ROUTES

//Root
router.get('/', (req, res) => {
    res.json({"message": "This is the root of the users route."});
});

// Create a new Page
router.post('/addPage', pages.create);

// Retrieve all Pages of a single workspace
router.post('/allWorkspacePages', pages.findAllWorkspacePages);

// Retrieve a single page with a pageID
router.post('/findPage', pages.findOne);

// Update a page with a pageID
router.put('/updatePage', pages.update);

// Delete a page with a pageID
router.post('/deletePage', pages.delete);

module.exports = router;