import joblib
import os

# Load the model and vectorizer
model_path = "app/model_output/spam_classifier.joblib"
vectorizer_path = "app/model_output/tfidf_vectorizer.joblib"

model = joblib.load(model_path)
vectorizer = joblib.load(vectorizer_path)

def predict_spam(text: str) -> bool:
    X = vectorizer.transform([text])
    y_pred = model.predict(X)
    return bool(y_pred[0])

