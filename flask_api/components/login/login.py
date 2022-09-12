from flask import Flask, request
from flask_restful import Resource, Api, reqparse
import pandas as pd
import ast
import json

class Login(Resource):
    def get(self):
        args = request.args
        
        name = args.get("name")
        pw = args.get("password")

        is_admin = False
        admin_account = None

        accounts = open("W:/ModLauncher/flask_api/components/login/accounts.json", "r")
        accounts_ = json.loads(accounts.read())

        for account in accounts_["admin"]:
            if name != account:
                is_admin = False
                admin_account = account
            else:
                is_admin = True
        
        if is_admin:
            print(f"- Account is admin -- {admin_account} -- {name}")
            return {"success": "login successful", "method": "admin()", "allowed_menus": "null" }
        else:
            for account in accounts_["standard"]:
                if name != account:
                    return {"failure": "Either account or password wasnt found"}
                else:
                    print(f"- Account is standard -- {name}")
                    return {"success": "login successful", "method": "show_allowed_menus()", "allowed_menus": ["minecraft", "titanfall2", "terraria", "plutonium_bo2", "plutonium_bo1"] }

    def post(self):
        args = request.args
        name = args.get("name")
        pw = args.get("password")

        standard_layout = {
            str(name):{
                "password": str(pw)
            }
        }

        accounts = open("W:/ModLauncher/flask_api/components/login/accounts.json", "r+")
        accounts_ = json.load(accounts)
        accounts_["standard"].append(standard_layout)
        accounts_.seek(0)
        json.dump(accounts_, accounts, indent=4)


class ChangePassword(Resource):
    def post(self):
        args = request.args

        #TODO: Verify is the user is the actual owner of the account
