import dearpygui.dearpygui as dpg
import requests
import json

#def json_to_list(json_format):
    #name_list = []
    #url_list = []

    #url_list_unformatted = json.loads(json_format)

    #for thing in url_list_unformatted:
    #    url_list.append(thing)

    #return url_list


def show_minecraft_menu():
    req_url = "http://127.0.0.1:5000/avalmods?service=minecraft&version=1.18.2"
    recv_json = requests.get(req_url).json()
    data = json.loads(recv_json)
    data_dump = json.dumps(data)
    url_list = []

    for item in data_dump:
        url_list.append(item)

    with dpg.window(label="Minecraft Mod Pack", width=625, height=470, no_resize=False):
        dpg.add_button(label="Download and Install")
        dpg.add_listbox(url_list)


def main():
    print("Client started...")

    dpg.create_context()
    dpg.create_viewport(title='Mod Launcher', width=640, height=480, resizable=False)
    dpg.setup_dearpygui()
    dpg.viewport_menu_bar()

    #with dpg.window(label="Mod Launcher", width=640, height=480, no_resize=True, no_collapse=True):
    #    dpg.add_text("Main Menu")
    #    dpg.add_button(label="Get Mods")

    with dpg.viewport_menu_bar():
        with dpg.menu(label="Games"):
            dpg.add_menu_item(label="Minecraft", callback=show_minecraft_menu)

    with dpg.viewport_menu_bar():
        with dpg.menu(label="Game Options"):
            dpg.add_menu_item(label="Update all")
            dpg.add_menu_item(label="Remove all")

if __name__ == '__main__':
    main()

    dpg.show_viewport()
    dpg.start_dearpygui()
    dpg.destroy_context()