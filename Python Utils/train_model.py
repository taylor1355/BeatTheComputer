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
    if os.path.isfile(MODEL_FILE_NAME):
        print("Loaded existing model")
        model = load_model(MODEL_FILE_NAME)
    else:
        print("Created new model")
        model = create_model()
    
    for i in range(100):
        model.fit(train_x, train_y, epochs=10)
        test_loss, test_acc = model.evaluate(test_x, test_y)
        print("Testing Set: - loss: " + str(test_loss) + " - acc: " + str(test_acc))
        model.save(MODEL_FILE_NAME)
        print()
    
def create_model():
    model = Sequential()
    model.add(Dense(2 * num_features, input_dim=num_features, activation='relu'))
    model.add(Dense(2 * num_features, activation='relu'))
    model.add(Dense(2 * num_features, activation='relu'))
    model.add(Dense(1, activation='sigmoid'))
    model.compile(loss='mean_squared_error', optimizer='adam', metrics=['accuracy'])
    return model
        
def get_features(line):
    features_start, features_end = line.index("["), line.index("]")
    features = line[features_start + 1 : features_end]
    return np.fromstring(features, sep=",")
    
def get_label(line):
    label_start = line.index(":") + 1
    return float(line[label_start:]) * 2 - 1

if __name__ == "__main__":
    main()