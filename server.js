var express = require('express');
var path = require('path');
var bodyParser = require('body-parser');
const say = require('say');
var mongoose = require('mongoose');
var Text = require('./Text.js');

var app = express();
var mongoDB = "mongodb://localhost/adidas";
mongoose.connect(mongoDB);

mongoose.Promise = global.Promise;

var db = mongoose.connection;

db.on('error', console.error.bind(console, 'MongoDB connection error:'));

const PORT = 1080;

app.use(bodyParser.urlencoded({ extended: false }))
app.use(bodyParser.json())

app.use(function(req, res, next) {
    res.header('Access-Control-Allow-Origin', '*');
    res.header('Access-Control-Allow-Headers', 'Origin, X-Requested-With, Content-Type, Accept');
    next();
  });

app.get('/', function(req, res){
    res.render(path.join(__dirname+'/index.html'))
})

app.post('/new/text', function(req, res){
    var id = req.body.id;
    var text = req.body.text;
    
    Text.findOne({id}, function(err, obj){
        if(!obj){
            var textSchema = new Text({
                id,
                text
            });
            
            textSchema.save(function(err){
                if(err){
                    res.json({
                        message: "error"
                    })
                }else{
                    res.json({
                        message:"OK"
                    })
                }
            })
        }else{
            var query = {id};
            Text.findOneAndUpdate(query, {id, text}, {upsert:true}, function(err, doc){
                if(err) res.json({status: -1, message: "Error"})
                res.json({
                    status: 0,
                    message: "Mensaje actualizado"
                })
            })
        }
    })
});

app.get('/id/:classId', function(req, res){
    var classId = parseInt(req.params.classId);
    if(!classId){
        res.json({
            status: -1,
            message: "ERROR, no text param"
        });
    }
    Text.findOne({id: classId}, function(err, doc){
        if(!doc){
            res.json({
                status: -1,
                message: "Error base de datos"
            })
        }else{
            say.speak(doc.text);
            res.json({
                status: 0,
                message: "OK"
            });
        }
    })
});

app.post('/update/texts', function(req, res){
    if(req.body.texts){
        var texts = req.body.texts;
        texts.forEach(element => {
            Text.findOne({id: element.id}, function(err, obj){
                if(!obj){
                    var textSchema = new Text({
                        id: element.id,
                        text: element.text
                    });
                    
                    textSchema.save(function(err){
                        if(err){
                            console.log("error");
                        }
                    })
                }else{
                    var query = {id: element.id};
                    Text.findOneAndUpdate(query, {id: element.id, text: element.text}, {upsert:true}, function(err, doc){
                        if(err)  console.log("error");
                    })
                }
            })
        });
        res.json({
            status: 0,
            message: "Mensajes actualizado"
        })
    }else{
        res.json({
            status: -1,
            message: "No data"
        })
    }
});

app.listen(PORT, function(err){
    if(err) console.log(err);
    console.log("app listening on port: "+PORT);
});