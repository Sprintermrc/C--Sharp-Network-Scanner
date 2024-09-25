
import cors from 'cors';
import { Server } from 'socket.io';
import { join } from 'path';
import express from 'express';
import bodyParser from 'body-parser';
//import { json } from 'body-parser';
import { createServer, request } from 'http';
import require from 'express';
import { fileURLToPath } from 'url';
import { dirname } from 'path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);
const app = express();
const server = createServer(app);
const io = new Server(server, {
    connectionStateRecovery: {},
});


app.use(express.json());
// app.use(cors());
// app.use((req, res, next) => {
//     res.setHeader('Access-Control-Allow-Origin', 'http://localhost:3200');
//     res.setHeader('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');
//     res.setHeader('Access-Control-Allow-Headers', 'X-Requested-With,content-type');
//     res.setHeader('Access-Control-Allow-Credentials', true);
//     next();
//   });

////////////////////////////////////////////////
//REST STUFF



app.get('/chat', (req, res) => {
    res.sendFile(join(__dirname, 'indexvl.html'));
});


app.get('/home', (req, res) => {
    console.log('[GET ROUTE]');
    res.sendFile(join(__dirname, 'home.html'));
});


app.use('/home', function (request, response) {
    Response.redirect('chat');
});


app.get('/name', (req, res) => {
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
    // if (message != null && message != '')
    // {
    io.emit('chat message', `Собеседник: ${message}`);
    if (!message) {
        return res.status(400).json({
        error: 'Message not found'});
       }
    console.log('Recieved Message:', message);
    // res.setHeader('Access-Control-Allow-Origin', 'http://localhost:3200');
    // res.setHeader('Access-Control-Allow-Methods', 'GET, POST, OPTIONS, PUT, PATCH, DELETE');
    // res.setHeader('Access-Control-Allow-Headers', 'X-Requested-With,content-type');
    // res.setHeader('Access-Control-Allow-Credentials', true);
    res.status(200).json({ message: 'Message recieved' });
    res.sendStatus(200).json({success: true});

   // }
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
        console.log('log: chat message: ' + msg);
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


//отправление запроса c сообщением на сервер
////////////////////////////


// const options = {
//     hostname: '',
//     port: 3300,
//     path: '/message',
//     method: 'POST',
//     headers: {
//         'Content-Type': 'application/json'
//     }
// }


// http = require('http');

// const message = io.on('connection', (socket) =>{
//   socket.on('chat message', (msg) => {


    //   const data = JSON.stringify({
    //     message: msg
    //   });
//     const req = http.request(options, (res) => {

//       console.log(`request status: ${res.statusCode}`);
//       let response = '';
//       let body = '';
//       res.on('data', (chunk) => {
//           response += chunk;
//       });
//       res.on('end', () => {
//           console.log('server response: ', response);
//       });

//     });
//     req.on('error', (error) => {
//         console.log(error);
//     });
//     //console.log(`*-*-*-*-*-*-**-*-*-**-*-msg ${Message}`);
//     req.write(data);
//     req.end();
//   });
// });





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
// const hostname = '172.29.9.90';
// const port = 3300;
// const server = http.createServer((req, res) => {
//     res.statusCode = 200;
//     res.setHeader('Content-Type', 'text/plain');
//     res.end('dsgfds');
// });

server.listen(3300, () => {
    console.log('----------------2 server running at http://localhost:3300');
});