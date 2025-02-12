function togglePasswordVisibility() {
    const passwordField = document.getElementById('Password');
    const showPasswordToggle = document.getElementById('showPasswordToggle');

    if (showPasswordToggle.checked) {
        passwordField.type = 'text';
    } else {
        passwordField.type = 'password';
    }
}