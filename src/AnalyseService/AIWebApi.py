from flask import Flask, jsonify, request
import tensorflow as tf
import numpy as np
import cv2
import os
import joblib
from flask_cors import CORS
app = Flask(__name__)
CORS(app)
# Paths for the models
person_detection_model_path = "person_detection_model.keras"
spam_detection_model_path = "spam_detection_model.pkl"
tfidf_vectorizer_path = "tfidf_vectorizer.pkl"

# Load the person detection model
person_detection_model = tf.keras.models.load_model(person_detection_model_path)

# Load the spam detection model and vectorizer
spam_detection_model = joblib.load(spam_detection_model_path)
tfidf_vectorizer = joblib.load(tfidf_vectorizer_path)

@app.route('/detect_person', methods=['POST'])
def detect_person():
    if 'image' not in request.files:
        return jsonify({"error": "No image file provided"}), 400
    
    image_file = request.files['image']
    image_path = "temp_image.jpg"
    image_file.save(image_path)
    
    # Load and preprocess the image
    img = cv2.imread(image_path)
    if img is None:
        os.remove(image_path)
        return jsonify({"error": "Invalid image file"}), 400
    
    img = cv2.resize(img, (128, 128))
    img = img / 255.0
    img = np.expand_dims(img, axis=0)
    
    # Predict using the person detection model
    prediction = person_detection_model.predict(img)[0][0]
    is_person = prediction >= 0.5
    
    os.remove(image_path)

    return jsonify({
        "is_person": bool(is_person)
    })

@app.route('/predict_spam', methods=['POST'])
def predict_spam():
    data = request.json
    if 'text' not in data:
        return jsonify({"error": "No text provided"}), 400
    
    email_text = data['text']
    text_tfidf = tfidf_vectorizer.transform([email_text])
    prediction = spam_detection_model.predict(text_tfidf)
    result = 'spam' if prediction[0] == 1 else 'not spam'
    
    return jsonify({
        "prediction": result
    })

if __name__ == '__main__':
    app.run(debug=True)
