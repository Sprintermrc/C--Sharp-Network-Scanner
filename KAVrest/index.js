const express = require('express');
const { createServer, request } = require('http');
const { join } = require('path');
const { Server } = require('socket.io');



const app = express();
const server = createServer(app);
const io = new Server(server, {
    connectionStateRecovery: {},

});


app.use(express.json());

////////////////////////////////////////////////
//REST STUFF



app.get('/chat', (req, res) => {
    res.sendFile(join(__dirname, 'index.html'));
});


app.get('/home', (req, res) => {
    console.log('[GET ROUTE]');
    res.sendFile(join(__dirname, 'home.html'));
});


app.use('/home', function (request, response) {
    Response.redirect('chat');
});


app.get('/server-info', (req, res) => {
    const hostName = require('os').hostname();
    const interfaces = require('os').networkInterfaces();
    let ipAdress = '';
    for (const networkInterface of Object.values(interfaces)) {
        for (const adress of networkInterface) {
            if (adress.family === 'IPv4' && !adress.internal) {
                ipAdress = adress.address;
                break;
            }
        }
    }
    res.json({
        hostname: hostName,
        ipAdress: ipAdress
    });
});


app.post('/message', (req, res) => {
    const message = req.body.message;
    console.log(`message:${message}`);
    if (!message) {
        return res.status(400).json({
        error: 'Message not found'});
       }
    console.log('Recieved Message:', message);
    res.status(200).json({ message: 'Message recieved' });
});

//END OF REST STUFF
///////////////////////////////////////////////


io.on('connection', (socket) => {
    console.log('a user connected');
    io.emit('chat message', 'user connected');
    socket.on('disconnect', () => {
        console.log('user disconnected');
        io.emit('chat message', 'user disconnected');

    });
});

io.on('connection', (socket) => {
    socket.on('chat message', (msg) => {
        console.log('message: ' + msg);
    });
});

io.emit('hello', 'world');

io.on('connection', (socket) => {
    socket.on('hello', (arg) => {
        console.log(arg); // 'world'
    });
});

io.on('connection', (socket) => {
    socket.emit('hello', 'world');
});

io.on('connection', (socket) => {
    socket.broadcast.emit('hi');
});

io.on('connection', (socket) => {
    socket.on('chat message', (msg) => {
        io.emit('chat message', msg);
    });
});


//отправление запроса на сервер
////////////////////////////
// https = require('https');
http = require('http');

const options = {
    hostname: '',
    port: 3400,
    path: '/message',
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
    }
}

const data = JSON.stringify({
    key: 'value',

});


const req = http.request(options, (res) => {
    console.log(`request status: ${res.statusCode}`);
    let data = '';
    res.on('data', (chunk) => {
        data += chunk;
    });
    res.on('end', () => {
        console.log(data);
    });
});
req.on('error', (error) => {
    console.log(error);
});
req.write(data);
req.end();
///////////////////////////




// io.on('connection', (socket) => {
//     socket.on('request', (arg1, arg2, callback) => {
//         console.log(arg1); // { foo: 'bar' }
//         console.log(arg2); // 'baz'
//         callback({
//             status: 'ok'
//         });
//     });
// });


server.listen(3300, () => {
    console.log('server running at http://localhost:3300');
});


'use strict';

const { networkInterfaces } = require('os');
const { hostname } = require('os');

const nets = networkInterfaces();
const results = Object.create(null);


io.on('connection', (socket) => {
    for (const name of Object.keys(nets)) {
        for (const net of nets[name]) {
            const familyV4Value = typeof net.family === 'string' ? 'IPv4' : 4
            if (net.family === familyV4Value && !net.internal) {
                if (!results[name]) {
                    results[name] = [];
                }
                results[name].push(net.address);
                io.emit('chat message', results[name]);
            }
        }
    }
});