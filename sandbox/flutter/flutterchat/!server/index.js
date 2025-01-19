const io = require("socket.io")(
  require("http").createServer(function() {}).listen(8080)
)

const users = {}
const rooms = {}

io.on("connection", io => {
  console.log("New connection!")

  io.on("validate", (data, callback) => {
    console.log("validate", data, users, rooms)
    const json = JSON.parse(data)
    const user = users[json.userName]
    if (user) {
      callback({status: user.password === json.password ? "ok" : "incorrect password"})
    } else {
      users[json.userName] = json
      io.broadcast.emit("newUser", users)
      callback({status: "created"})
    }
  })

  io.on("create", (data, callback) => {
    console.log("create", data, users, rooms)
    const json = JSON.parse(data)
    if (rooms[json.roomName]) {
      callback({status: "exists"})
    } else {
      json.users = {}
      rooms[json.roomName] = json
      io.broadcast.emit("created", rooms)
      callback({status: "created", rooms: rooms})
    }
  })

  io.on("listRooms", (data, callback) => {
    console.log("listRooms", data, users, rooms);
    callback(rooms)
  })

  io.on("listUsers", (data, callback) => {
    console.log("listUsers", data, users, rooms);
    callback(users)
  })

  io.on("join", (data, callback) => {
    console.log("join", data, users, rooms)
    const json = JSON.parse(data)
    const room = rooms[json.roomName]
    if (Object.keys(room.users).length >= 10) {
      callback({status: "full"})
    } else {
      room.users[json.userName] = users[json.userName]
      io.broadcast.emit("joined", room)
      callback({status: "joined", room: room})
    }
  })

  io.on("post", (data, callback) => {
    console.log("post", data, users, rooms)
    const json = JSON.parse(data)
    io.broadcast.emit("posted", json)
    callback({status: "ok"})
  })

  io.on("invite", (data, callback) => {
    console.log("invite", data, users, rooms)
    const json = JSON.parse(data)
    io.broadcast.emit("invited", json)
    callback({status: "ok"})
  })

  io.on("leave", (data, callback) => {
    console.log("leave", data, users, rooms)
    const json = JSON.parse(data)
    const room = rooms[json.roomName]
    delete room.users[json.userName]
    io.broadcast.emit("left", json)
    callback({status: "ok"})
  })

  io.on("close", (data, callback) => {
    console.log("close", data, users, rooms)
    const json = JSON.parse(data)
    delete rooms[json.roomName]
    io.broadcast.emit("closed", {roomName: json.roomName, rooms: rooms})
    callback(rooms)
  })

  io.on("kick", (data, callback) => {
    console.log("kick", data, users, rooms)
    const json = JSON.parse(data)
    const room = rooms[json.roomName]
    const roomUsers = room.users
    delete roomUsers[json.userName]
    io.broadcast.emit("kicked", room)
    callback({status: "ok"})
  })
})
