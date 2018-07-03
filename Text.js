const mongoose = require('mongoose');
const Schema = mongoose.Schema;

const textSchema = new Schema({
    id: Number,
    text: String
});

module.exports = mongoose.model("Text", textSchema);