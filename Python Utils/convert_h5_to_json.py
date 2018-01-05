import KerasModelToJSON as js
import os.path
import sys

if len(sys.argv) >= 2:
    h5_file = sys.argv[1]
    valid_file = True
    if not os.path.isfile(h5_file):
        print("Must pass in a valid file")
        valid_file = False
    elif not h5_file.endswith(".h5"):
        print("Must pass in a .h5 file")
        valid_file = False
    
    if valid_file:
        from keras.models import load_model
        keras_model = load_model(h5_file)
        json_file = h5_file.replace(".h5", ".json")
        json_writer = js.JSONwriter(keras_model, json_file)
        json_writer.save()
else:
    print("Missing required argument")