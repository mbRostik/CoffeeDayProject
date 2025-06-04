import pandas as pd
import numpy as np
import joblib
import os
import argparse

from sklearn.model_selection import train_test_split
from sklearn.preprocessing import LabelEncoder
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.linear_model import LogisticRegression
from sklearn.metrics import (
    accuracy_score,
    precision_score,
    recall_score,
    f1_score,
    classification_report,
    confusion_matrix
)

def load_and_prepare_data(filepath: str) -> pd.DataFrame:
    if not os.path.exists(filepath):
        raise FileNotFoundError(f"File '{filepath}' not found.")

    df = pd.read_csv(filepath, encoding='ISO-8859-1')
    df = df[['v1', 'v2']]
    df.columns = ['label', 'message']

    if df.isnull().sum().sum() > 0:
        raise ValueError("Dataset contains missing values.")

    encoder = LabelEncoder()
    df['label'] = encoder.fit_transform(df['label'])  # Ham = 0, Spam = 1

    return df

def split_data(df: pd.DataFrame):
    return train_test_split(
        df['message'], df['label'],
        test_size=0.2,
        stratify=df['label'],
        random_state=42
    )

def train_spam_classifier(X_train, y_train):
    vectorizer = TfidfVectorizer(
        stop_words='english',
        lowercase=True,
        max_df=0.9,
        min_df=2
    )

    X_train_tfidf = vectorizer.fit_transform(X_train)

    model = LogisticRegression(
        solver='liblinear',
        C=1.0,
        class_weight='balanced',
        random_state=42
    )

    model.fit(X_train_tfidf, y_train)

    return model, vectorizer

def evaluate_model(model, vectorizer, X_test, y_test):
    X_test_tfidf = vectorizer.transform(X_test)
    y_pred = model.predict(X_test_tfidf)

    print("=== Evaluation Metrics ===")
    print(f"Accuracy : {accuracy_score(y_test, y_pred):.4f}")
    print(f"Precision: {precision_score(y_test, y_pred):.4f}")
    print(f"Recall   : {recall_score(y_test, y_pred):.4f}")
    print(f"F1 Score : {f1_score(y_test, y_pred):.4f}")
    print("\n=== Classification Report ===")
    print(classification_report(y_test, y_pred, target_names=["Ham", "Spam"]))
    print("\n=== Confusion Matrix ===")
    print(confusion_matrix(y_test, y_pred))

def save_model(model, vectorizer, output_dir="model_output"):
    os.makedirs(output_dir, exist_ok=True)
    joblib.dump(model, os.path.join(output_dir, "spam_classifier.joblib"))
    joblib.dump(vectorizer, os.path.join(output_dir, "tfidf_vectorizer.joblib"))
    print(f"\n✅ Model and vectorizer saved to: {output_dir}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Train a spam detection model.")
    parser.add_argument("--data", type=str, default="spam.csv", help="Path to spam.csv")
    args = parser.parse_args()

    print("🔄 Loading data...")
    df = load_and_prepare_data(args.data)

    print("🔄 Splitting data...")
    X_train, X_test, y_train, y_test = split_data(df)

    print("🔄 Training model...")
    model, vectorizer = train_spam_classifier(X_train, y_train)

    print("🔍 Evaluating model...")
    evaluate_model(model, vectorizer, X_test, y_test)

    print("💾 Saving model and vectorizer...")
    save_model(model, vectorizer)
