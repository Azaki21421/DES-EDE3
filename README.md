# USB Work - [Triple DES](https://en.wikipedia.org/wiki/Triple_DES) File Encryption and Decryption

This is a simple Windows Forms application built in C# that demonstrates the use of Triple DES (3DES) encryption and decryption on files. It allows the user to drag and drop a file, then encrypt or decrypt it using a series of password prompts.

## Features

- **Drag and Drop**: Drag and drop a file to encrypt or decrypt.
- **Triple DES Encryption**: Encrypt files using three different passwords in a three-step process with Triple DES encryption.
- **Decryption**: Decrypt files using the same three passwords, reversing the encryption steps.
- **Password Prompts**: Users are prompted to enter three different passwords for encryption and decryption.
- **File Output**: Encrypted files are saved with an `.enc` extension, while decrypted files are saved with `.dec`.

## How to Use

1. **Clone the repository**:
   ```bash
   git clone https://github.com/Azaki21421/DES-EDE3.git

2. Build the application: Open the project in Visual Studio and build it. (or skip and just run app from [release](https://github.com/Azaki21421/DES-EDE3/releases))

3. Run the application:

- Launch the application.
- Drag and drop a file onto the form.
- Click the "Encrypt" button to encrypt the file.
- Click the "Decrypt" button to decrypt the file.
- Enter the required passwords when prompted.

4. Encryption:

- The program will ask for three passwords for encryption.
- The file will be encrypted and saved with a .enc extension.

5. Decryption:

- The program will ask for the same three passwords for decryption.
- The file will be decrypted and saved with a .dec extension.

6. Passwords
- The application uses three passwords for encryption and decryption. The passwords are used to generate keys with SHA256 and then truncated to 24 bytes for the Triple DES algorithm.

## Dependencies
- .NET Framework (Windows Forms)
