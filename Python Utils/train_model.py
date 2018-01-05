from keras.models import Sequential, load_model
from keras.layers import Dense
import KerasModelToJSON as js
import numpy as np
import h5py
import random
import os.path
import os
import sys

TRAINING_FRACTION = 0.9
BATCH_SIZE = 3
MODEL_FILE_EXTENSION = ".h5"
EXAMPLE_FILE_EXTENSION = ".example"

def main(directory, epochs):
    example_file_name = None
    model_file_name = None
    for file in os.listdir(directory):
        if file.endswith(EXAMPLE_FILE_EXTENSION):
            example_file_name = file
        elif file.endswith(MODEL_FILE_EXTENSION):
            model_file_name = file
            
    if example_file_name is None:
        print("No example files found in directory " + directory)
        return
    else:
        print("Example file " + example_file_name + " found")

    with open(example_file_name) as example_file:
        x, y = list(), list()
        for line in example_file:
            x.append(get_features(line))
            y.append(get_label(line))
        
    num_features = len(x[0])
    num_examples = len(x)
    num_training = int(TRAINING_FRACTION * num_examples)
    num_testing = num_features - num_training
    
    indices = list(range(num_examples))
    random.shuffle(indices)
    
    train_x = np.array([x[i] for i in indices[0:num_training]])
    train_y = np.array([y[i] for i in indices[0:num_training]])
    test_x = np.array([x[i] for i in indices[0:num_testing]])
    test_y = np.array([y[i] for i in indices[0:num_testing]])
        
    model = None
    best_test_loss = None
    if model_file_name is None:
        model = create_model(num_features)
        model_file_name = directory + "/model" + MODEL_FILE_EXTENSION
        print("Created new model")
    else:
        model = load_model(model_file_name)
        print("Loaded existing model")
        best_test_loss = model.evaluate(test_x, test_y)
    
    iterations = int(epochs / BATCH_SIZE)
    for i in range(iterations):
        start_epoch = i * BATCH_SIZE
        print("\nEpochs " + str(start_epoch) + "-" + str(start_epoch + BATCH_SIZE - 1))
        model.fit(train_x, train_y, epochs=BATCH_SIZE)
        test_loss = model.evaluate(test_x, test_y)
        print("Testing Set Loss: " + str(test_loss))
        print("Best Testing Set Loss: " + str(best_test_loss))
        if best_test_loss is None or test_loss < best_test_loss:
            print("Model exceeds previous best, saving...")
            save_model(model, model_file_name)           
            best_test_loss = test_loss
            print("Successfully saved")
            
            random_indices = np.random.choice(len(test_x), 10)
            for index in random_indices:
                y_prediction = model.predict(np.reshape(test_x[index], (-1, num_features)))[0]
                print(str(test_x[index]) + " predicted = " + str(y_prediction) + ", actual = " + str(test_y[index]))
        
    if not os.path.isfile(model_file_name):
        save_model(model, model_file_name)

def save_model(model, model_file_name):
    model.save(model_file_name)
    json_model_file_name = os.path.splitext(model_file_name)[0] + ".json"
    json_writer = js.JSONwriter(model, json_model_file_name)
    json_writer.save()

def create_model(num_features):
    model = Sequential()
    model.add(Dense(num_features, input_dim=num_features, activation='relu'))
    model.add(Dense(num_features, activation='relu'))
    model.add(Dense(num_features, activation='relu'))
    model.add(Dense(num_features, activation='relu'))
    model.add(Dense(num_features, activation='relu'))
    model.add(Dense(num_features, activation='relu'))
    model.add(Dense(num_features, activation='relu'))
    model.add(Dense(1, activation='sigmoid'))
    model.compile(loss='mean_squared_error', optimizer='adam')
    return model
        
def get_features(line):
    features_start, features_end = line.index("[") + 1, line.index("]")
    features = line[features_start : features_end]
    return np.fromstring(features, sep=",")
    
def get_label(line):
    label_start = line.index(":") + 1
    return float(line[label_start:])
    
if len(sys.argv) >= 3:
    directory = sys.argv[1]
    epochs = int(sys.argv[2])
    valid_arguments = True
    if not os.path.isdir(directory):
        print("Must pass in a valid directory")
        valid_arguments = False
    if epochs < 0:
        print("Training epochs must be non-negative")
        valid_arguments = False
    
    if valid_arguments:
        main(directory, epochs)
else:
    print("Missing required arguments")