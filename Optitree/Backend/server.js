const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');

// create express app
const app = express();

// parse requests of content-type - application/x-www-form-urlencoded with a increased size limit of 50mb
app.use(bodyParser.urlencoded({ limit: '50mb', extended: true }))

// parse requests of content-type - application/json with a increased size limit of 50mb
app.use(bodyParser.json({limit: '50mb', extended: true}))

//Allow cors
app.use(cors());

/*---BELOW ARE ALL THE ROUTES---*/

//Root route
app.get('/', (req, res) => {
    res.json({"message": "Welcome to the Optitree API application. You can use this API to interact with the Optitree database."});
});
//Login route
loginRoute = require('./controllers/authentication.controller');
app.post('/login', loginRoute.authenticateUser);
//User CRUD routes
const userRoute = require('./routes/user.routes');
app.use('/users', userRoute);
//Workspace CRUD routes
const workspaceRoutes = require('./routes/workspace.routes ');
app.use('/workspaces', workspaceRoutes);
//Page CRUD routes
const pageRoutes = require('./routes/page.routes');
app.use('/pages', pageRoutes);

// Listen for requests
app.listen(3000, () => {
    console.log("Server is listening on port 3000");
});
