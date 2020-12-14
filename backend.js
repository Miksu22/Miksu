const express = require('express')
const app = express()
const port = 3000
var mongo = require('mongodb');
var dbClient = mongo.MongoClient;

var url = "mongodb://localhost:27017/test";

dbClient.connect(url, function(err, db) {
	if (err) throw err;
  
	app.get('/helloworld', (req, res) => {
	  res.send('Hello World!')
	})

	app.get('/list', (req, res) => {
	  var dbo = db.db("test");
	  dbo.collection("test").find().toArray(function(err, result) {
		if (err) throw err;
		res.send(result);
	  });
	})

	app.get('/add', (req, result) => {
	    var dbo = db.db("test");
	    var myobj = { testidata: "Morjesta kaikille ukoille!" };
	    dbo.collection("test").insertOne(myobj, function(err, res) {
		if (err) throw err;
		result.send("Lisätty 1 rivi testidataan");
	    });
	})
	
	app.listen(port, () => {
	  console.log('Example app listening at http://localhost:' + port)
	})

});