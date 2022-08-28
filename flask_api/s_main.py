from flask import Flask
from flask_restful import Resource, Api, reqparse
import pandas as pd
import ast

app = Flask(__name__)
api = Api(app)

class Mods(Resource):
    def get(self):
        data = "No mods to return at the moment"
        return {'data': data}, 200