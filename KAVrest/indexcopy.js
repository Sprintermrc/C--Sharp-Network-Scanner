const express = require('express');
const { createServer } = require('node:http');
const { join } = require('node:path');
const { Server } = require('socket.io');

const app = express();
const server = createServer(app);
const io = new Server(server);

//REST STUFF
app.get('/', (req, res) => {
    res.sendFile(join(__dirname, 'index copy.html'));
});

//END OF REST STUFF


io.on('connection', (socket) => {
    console.log('a user connected');
    io.emit('chat message', 'user connected')
    socket.on('disconnect', () => {
      console.log('user disconnected');
      io.emit('chat message', 'user disconnected')

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

io.on('connection', (socket) => {
    socket.on('request', (arg1, arg2, callback) => {
        console.log(arg1); // { foo: 'bar' }
        console.log(arg2); // 'baz'
        callback({
            status: 'ok'
        });
    });
});
  
server.listen(3800, () => {
  console.log('server running at http://localhost:3800');
});