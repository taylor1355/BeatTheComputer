from keras.models import Sequential, load_model
from keras.layers import Dense
import numpy as np
import h5py
import random
import os.path

TRAINING_FRACTION = 0.9
MODEL_FILE_NAME = "model.h5"
EXAMPLE_FILE_NAME = "examples.txt"

def main():
    with open(EXAMPLE_FILE_NAME) as example_file:
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
    if os.path.isfile(MODEL_FILE_NAME):
        model = load_model(MODEL_FILE_NAME)
        print("Loaded existing model")
        best_test_loss = model.evaluate(test_x, test_y)
    else:
        model = create_model(num_features)
        print("Created new model")
    
    for i in range(1000):
        model.fit(train_x, train_y, epochs=2)
        test_loss = model.evaluate(test_x, test_y)
        print("Testing Set Loss: " + str(test_loss))
        print("Best Testing Set Loss: " + str(best_test_loss))
        if best_test_loss is None or test_loss < best_test_loss:
            print("Model exceeds previous best, saving...")
            model.save(MODEL_FILE_NAME)
            best_test_loss = test_loss
            print("Successfully saved")
            
            random_indices = np.random.choice(len(test_x), 10)
            for index in random_indices:
                y_prediction = model.predict(np.reshape(test_x[index], (-1, num_features)))[0]
                print(str(test_x[index]) + " predicted = " + str(y_prediction) + ", actual = " + str(test_y[index]))
        print()

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

if __name__ == "__main__":
    main()