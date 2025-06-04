import torch
from torchvision import models, transforms
from PIL import Image
import os
import torch.nn as nn

device = torch.device("cuda" if torch.cuda.is_available() else "cpu")

# === Завантаження моделі ===
model = models.efficientnet_b0(weights='IMAGENET1K_V1')
model.classifier = nn.Sequential(
    nn.Dropout(0.4),
    nn.Linear(model.classifier[1].in_features, 128),
    nn.ReLU(),
    nn.Linear(128, 1)
)

model_path = os.path.join(os.path.dirname(__file__), "efficientnet_face_model.pth")
model.load_state_dict(torch.load(model_path, map_location=device))
model.to(device)
model.eval()

# === Предобробка зображення ===
transform = transforms.Compose([
    transforms.Resize((224, 224)),
    transforms.ToTensor(),
    transforms.Normalize([0.485, 0.456, 0.406],
                         [0.229, 0.224, 0.225])
])

def predict_face(image: Image.Image) -> bool:
    image_tensor = transform(image).unsqueeze(0).to(device)
    with torch.no_grad():
        output = model(image_tensor)
        prob = torch.sigmoid(output).item()
    return prob <= 0.5  # 🔁 інверсія: 0 (face) → True, 1 (no face) → False

