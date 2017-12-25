def main():
    feature_counts = {}
    line_count = 0
    with open("examples.txt") as file:
        for line in file:
            line_count += 1
            features_start, features_end = line.index("["), line.index("]")
            features = line[features_start + 1 : features_end]
            if features in feature_counts:
                feature_counts[features] += 1
            else:
                feature_counts[features] = 1
        
        sorted_occurrences = []
        for key, value in feature_counts.items():
            sorted_occurrences.append(str(value) + " occurrences of [" + key + "]")
        sorted_occurrences.sort(key=natural_sort_key)
        for i in range(len(sorted_occurrences)):
            occurrences = int(sorted_occurrences[i][:sorted_occurrences[i].index(" ")])
            if occurrences > 1:
                print(sorted_occurrences[i])
            
        print("Total Duplicates: " + str(line_count - len(feature_counts)))
            
import re
def natural_sort_key(s, _nsre=re.compile('([0-9]+)')):
    return [int(text) if text.isdigit() else text.lower()
            for text in re.split(_nsre, s)]    
    
if __name__ == "__main__":
    main()