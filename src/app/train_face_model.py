import os
import torch
import torch.nn as nn
import torch.optim as optim
from torchvision import datasets, transforms, models
from torch.utils.data import DataLoader
from sklearn.metrics import classification_report, confusion_matrix
import matplotlib.pyplot as plt
import numpy as np
from torch.optim.lr_scheduler import ReduceLROnPlateau

# ====== Device ======
device = torch.device("cuda" if torch.cuda.is_available() else "cpu")
print(f"\U0001F680 Using device: {device}")
if device.type == "cuda":
    print(f"\U0001F527 Device name: {torch.cuda.get_device_name(0)}")

# ====== Paths ======
TRAIN_DIR = r"C:\\Users\\Asus\\Desktop\\Dyploma\\Face_code\\data\\data\\train"
TEST_DIR = r"C:\\Users\\Asus\\Desktop\\Dyploma\\Face_code\\data\\data\\test"

# ====== Hyperparameters ======
IMG_SIZE = 224
BATCH_SIZE = 32
EPOCHS = 15
LR = 1e-4
PATIENCE = 3

# ====== Transforms ======
normalize = transforms.Normalize([0.485, 0.456, 0.406],
                                 [0.229, 0.224, 0.225])

train_transforms = transforms.Compose([
    transforms.Resize((IMG_SIZE, IMG_SIZE)),
    transforms.RandomHorizontalFlip(),
    transforms.ToTensor(),
    normalize
])

test_transforms = transforms.Compose([
    transforms.Resize((IMG_SIZE, IMG_SIZE)),
    transforms.ToTensor(),
    normalize
])

# ====== Datasets and Loaders ======
train_dataset = datasets.ImageFolder(TRAIN_DIR, transform=train_transforms)
test_dataset = datasets.ImageFolder(TEST_DIR, transform=test_transforms)

train_loader = DataLoader(train_dataset, batch_size=BATCH_SIZE, shuffle=True)
test_loader = DataLoader(test_dataset, batch_size=BATCH_SIZE, shuffle=False)

# ====== Model: EfficientNetB0 (transfer learning) ======
model = models.efficientnet_b0(weights='IMAGENET1K_V1')
for param in model.features.parameters():
    param.requires_grad = False

model.classifier = nn.Sequential(
    nn.Dropout(0.4),
    nn.Linear(model.classifier[1].in_features, 128),
    nn.ReLU(),
    nn.Linear(128, 1)
)
model = model.to(device)

# ====== Loss, Optimizer, Scheduler ======
criterion = nn.BCEWithLogitsLoss()
optimizer = optim.Adam(model.parameters(), lr=LR)
scheduler = ReduceLROnPlateau(optimizer, mode='min', factor=0.5, patience=2, verbose=True)

# ====== Training Loop with Early Stopping ======
train_losses = []
val_losses = []
best_val_loss = float('inf')
epochs_no_improve = 0
MODEL_PATH = "efficientnet_face_model.pth"

for epoch in range(EPOCHS):
    model.train()
    running_loss = 0.0

    for inputs, labels in train_loader:
        inputs, labels = inputs.to(device), labels.float().unsqueeze(1).to(device)

        optimizer.zero_grad()
        outputs = model(inputs)
        loss = criterion(outputs, labels)
        loss.backward()
        optimizer.step()

        running_loss += loss.item() * inputs.size(0)

    epoch_loss = running_loss / len(train_loader.dataset)
    train_losses.append(epoch_loss)

    # Validation
    model.eval()
    val_loss = 0.0
    preds = []
    targets = []

    with torch.no_grad():
        for inputs, labels in test_loader:
            inputs, labels = inputs.to(device), labels.float().unsqueeze(1).to(device)
            outputs = model(inputs)
            loss = criterion(outputs, labels)
            val_loss += loss.item() * inputs.size(0)

            preds.extend((torch.sigmoid(outputs) > 0.5).cpu().numpy())
            targets.extend(labels.cpu().numpy())

    val_epoch_loss = val_loss / len(test_loader.dataset)
    val_losses.append(val_epoch_loss)
    scheduler.step(val_epoch_loss)

    print(f"Epoch {epoch+1}/{EPOCHS} | Train Loss: {epoch_loss:.4f} | Val Loss: {val_epoch_loss:.4f}")

    # Early Stopping
    if val_epoch_loss < best_val_loss:
        best_val_loss = val_epoch_loss
        epochs_no_improve = 0
        torch.save(model.state_dict(), MODEL_PATH)
    else:
        epochs_no_improve += 1
        if epochs_no_improve >= PATIENCE:
            print(f"⏹️ Early stopping triggered at epoch {epoch+1}")
            break

print(f"\u2705 Best model saved to {MODEL_PATH}")

# ====== Metrics ======
print("\n\U0001F4CA Classification Report:")
print(classification_report(targets, preds, target_names=train_dataset.classes))

cm = confusion_matrix(np.array(targets).flatten(), np.array(preds).flatten())
print("Confusion Matrix:")
print(cm)

# ====== Loss Plot ======
plt.plot(train_losses, label='Train Loss')
plt.plot(val_losses, label='Validation Loss')
plt.xlabel('Epoch')
plt.ylabel('Loss')
plt.legend()
plt.title('Training & Validation Loss')
plt.show()

