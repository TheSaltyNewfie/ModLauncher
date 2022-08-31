from flask import Flask, request
from flask_restful import Resource, Api, reqparse
import pandas as pd
import ast
import json

app = Flask(__name__)
api = Api(app)
parser = reqparse.RequestParser()

class Mods(Resource):
    def get(self):
        args = request.args
        
        if args.get("service") == "minecraft":
            try:
                a = open(f'W:/ModLauncher/flask_api/configs/minecraft/minecraft_{args.get("version")}.json', "r")
                data = json.loads(a.read())
                return data["mods"], 200
            except Exception as e:
                return {'error': str(e)}, 500
        return {'you arent supposed to see this': '404'}, 404

api.add_resource(Mods, '/avalmods')

if __name__ == '__main__':
    app.run()