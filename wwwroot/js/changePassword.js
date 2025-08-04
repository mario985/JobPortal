// Change Password JavaScript

document.addEventListener('DOMContentLoaded', function() {
    const form = document.getElementById('changePasswordForm');
    const submitBtn = form.querySelector('.change-password-btn');
    
    // Add password visibility toggles
    addPasswordToggles();
    
    // Add password strength indicator
    addPasswordStrengthIndicator();
    
    // Form validation and submission
    form.addEventListener('submit', function(e) {
        e.preventDefault();
        
        if (validateForm()) {
            submitForm();
        }
    });
    
    // Real-time validation
    const inputs = form.querySelectorAll('.form-control');
    inputs.forEach(input => {
        input.addEventListener('blur', function() {
            validateField(this);
        });
        
        input.addEventListener('input', function() {
            clearFieldError(this);
            
            // Update password strength for new password field
            if (this.name === 'NewPassword') {
                updatePasswordStrength(this.value);
            }
            
            // Check password confirmation
            if (this.name === 'ConfirmPassword') {
                checkPasswordConfirmation();
            }
        });
    });
    
    // Add password visibility toggles
    function addPasswordToggles() {
        const passwordFields = form.querySelectorAll('input[type="password"]');
        
        passwordFields.forEach(field => {
            const wrapper = document.createElement('div');
            wrapper.className = 'password-field';
            
            // Move the field into the wrapper
            field.parentNode.insertBefore(wrapper, field);
            wrapper.appendChild(field);
            
            // Add toggle button
            const toggleBtn = document.createElement('button');
            toggleBtn.type = 'button';
            toggleBtn.className = 'password-toggle';
            toggleBtn.innerHTML = '<i class="fas fa-eye"></i>';
            
            toggleBtn.addEventListener('click', function() {
                const type = field.type === 'password' ? 'text' : 'password';
                field.type = type;
                
                const icon = this.querySelector('i');
                icon.className = type === 'password' ? 'fas fa-eye' : 'fas fa-eye-slash';
            });
            
            wrapper.appendChild(toggleBtn);
        });
    }
    
    // Add password strength indicator
    function addPasswordStrengthIndicator() {
        const newPasswordField = form.querySelector('[name="NewPassword"]');
        if (!newPasswordField) return;
        
        const strengthDiv = document.createElement('div');
        strengthDiv.className = 'password-strength';
        strengthDiv.innerHTML = `
            <div class="password-strength-text"></div>
            <div class="password-strength-bar">
                <div class="password-strength-fill"></div>
            </div>
        `;
        
        newPasswordField.parentNode.appendChild(strengthDiv);
    }
    
    // Update password strength
    function updatePasswordStrength(password) {
        const strengthDiv = document.querySelector('.password-strength');
        const strengthText = strengthDiv.querySelector('.password-strength-text');
        const strengthFill = strengthDiv.querySelector('.password-strength-fill');
        
        if (!password) {
            strengthDiv.style.display = 'none';
            return;
        }
        
        const strength = calculatePasswordStrength(password);
        
        strengthDiv.style.display = 'block';
        strengthDiv.className = `password-strength ${strength.level}`;
        strengthText.textContent = strength.message;
        strengthFill.className = `password-strength-fill ${strength.level}`;
    }
    
    // Calculate password strength
    function calculatePasswordStrength(password) {
        let score = 0;
        let feedback = [];
        
        // Length check
        if (password.length >= 8) {
            score += 1;
        } else {
            feedback.push('At least 6 characters');
        }
        
        // Uppercase check
        if (/[A-Z]/.test(password)) {
            score += 1;
        } else {
            feedback.push('One uppercase letter');
        }
        
        // Lowercase check
        if (/[a-z]/.test(password)) {
            score += 1;
        } else {
            feedback.push('One lowercase letter');
        }
        
        // Number check
        if (/\d/.test(password)) {
            score += 1;
        } else {
            feedback.push('One number');
        }
        
        // Special character check
        if (/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
            score += 1;
        } else {
            feedback.push('One special character');
        }
        
        // Determine strength level
        let level, message;
        if (score <= 2) {
            level = 'weak';
            message = 'Weak password';
        } else if (score <= 3) {
            level = 'medium';
            message = 'Medium strength password';
        } else {
            level = 'strong';
            message = 'Strong password';
        }
        
        return { level, message, score, feedback };
    }
    
    // Check password confirmation
    function checkPasswordConfirmation() {
        const newPassword = form.querySelector('[name="NewPassword"]').value;
        const confirmPassword = form.querySelector('[name="ConfirmPassword"]').value;
        
        if (confirmPassword && newPassword !== confirmPassword) {
            const confirmField = form.querySelector('[name="ConfirmPassword"]');
            showFieldError(confirmField, 'Passwords do not match');
        }
    }
    
    // Form validation function
    function validateForm() {
        let isValid = true;
        const inputs = form.querySelectorAll('.form-control');
        
        inputs.forEach(input => {
            if (!validateField(input)) {
                isValid = false;
            }
        });
        
        // Additional validation
        const newPassword = form.querySelector('[name="NewPassword"]').value;
        const confirmPassword = form.querySelector('[name="ConfirmPassword"]').value;
        
        if (newPassword !== confirmPassword) {
            const confirmField = form.querySelector('[name="ConfirmPassword"]');
            showFieldError(confirmField, 'Passwords do not match');
            isValid = false;
        }
        
        return isValid;
    }
    
    // Field validation
    function validateField(field) {
        const value = field.value.trim();
        const fieldName = field.name;
        let isValid = true;
        let errorMessage = '';
        
        // Clear previous errors
        clearFieldError(field);
        
        // Validation rules
        switch(fieldName) {
            case 'CurrentPassword':
                if (!value) {
                    errorMessage = 'Current password is required';
                    isValid = false;
                }
                break;
                
            case 'NewPassword':
                if (!value) {
                    errorMessage = 'New password is required';
                    isValid = false;
                } else if (value.length < 8) {
                    errorMessage = 'Password must be at least 6 characters long';
                    isValid = false;
                } else {
                    const strength = calculatePasswordStrength(value);
                    if (strength.score < 3) {
                        errorMessage = 'Password is too weak. Please include uppercase, lowercase, numbers, and special characters.';
                        isValid = false;
                    }
                }
                break;
                
            case 'ConfirmPassword':
                if (!value) {
                    errorMessage = 'Please confirm your new password';
                    isValid = false;
                } else {
                    const newPassword = form.querySelector('[name="NewPassword"]').value;
                    if (value !== newPassword) {
                        errorMessage = 'Passwords do not match';
                        isValid = false;
                    }
                }
                break;
        }
        
        if (!isValid) {
            showFieldError(field, errorMessage);
        } else {
            showFieldSuccess(field);
        }
        
        return isValid;
    }
    
    // Show field error
    function showFieldError(field, message) {
        field.classList.add('is-invalid');
        field.classList.remove('is-valid');
        
        const errorSpan = field.parentNode.querySelector('.text-danger');
        if (errorSpan) {
            errorSpan.textContent = message;
        }
    }
    
    // Show field success
    function showFieldSuccess(field) {
        field.classList.add('is-valid');
        field.classList.remove('is-invalid');
    }
    
    // Clear field error
    function clearFieldError(field) {
        field.classList.remove('is-invalid', 'is-valid');
        const errorSpan = field.parentNode.querySelector('.text-danger');
        if (errorSpan) {
            errorSpan.textContent = '';
        }
    }
    
    // Submit form via AJAX
    function submitForm() {
        // Show loading state
        submitBtn.classList.add('loading');
        submitBtn.disabled = true;
        
        const formData = new FormData(form);
        
        fetch(form.action, {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': form.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                showSuccessMessage(data.message);
                // Reset form
                form.reset();
                // Clear validation states
                inputs.forEach(input => {
                    input.classList.remove('is-valid', 'is-invalid');
                });
                // Clear password strength indicator
                const strengthDiv = document.querySelector('.password-strength');
                if (strengthDiv) {
                    strengthDiv.style.display = 'none';
                }
            } else {
                showErrorMessage(data.message || 'An error occurred while changing your password');
                
                // Handle field-specific errors
                if (data.errors) {
                    Object.keys(data.errors).forEach(fieldName => {
                        const field = form.querySelector(`[name="${fieldName}"]`);
                        if (field) {
                            showFieldError(field, data.errors[fieldName][0]);
                        }
                    });
                }
                
                // Handle identity errors
                if (data.identityErrors) {
                    data.identityErrors.forEach(error => {
                        showErrorMessage(error);
                    });
                }
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showErrorMessage('An error occurred while changing your password. Please try again.');
        })
        .finally(() => {
            // Remove loading state
            submitBtn.classList.remove('loading');
            submitBtn.disabled = false;
        });
    }
    
    // Show success message
    function showSuccessMessage(message) {
        const alertDiv = document.createElement('div');
        alertDiv.className = 'alert alert-success alert-dismissible fade show';
        alertDiv.innerHTML = `
            <i class="fas fa-check-circle me-2"></i>${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        const header = form.parentNode.querySelector('.change-password-header');
        header.parentNode.insertBefore(alertDiv, header.nextSibling);
        
        // Auto-dismiss after 5 seconds
        setTimeout(() => {
            if (alertDiv.parentNode) {
                alertDiv.remove();
            }
        }, 5000);
    }
    
    // Show error message
    function showErrorMessage(message) {
        const alertDiv = document.createElement('div');
        alertDiv.className = 'alert alert-danger alert-dismissible fade show';
        alertDiv.innerHTML = `
            <i class="fas fa-exclamation-circle me-2"></i>${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        const header = form.parentNode.querySelector('.change-password-header');
        header.parentNode.insertBefore(alertDiv, header.nextSibling);
        
        // Auto-dismiss after 5 seconds
        setTimeout(() => {
            if (alertDiv.parentNode) {
                alertDiv.remove();
            }
        }, 5000);
    }
    
    // Add smooth animations to form elements
    const formGroups = form.querySelectorAll('.form-group');
    formGroups.forEach((group, index) => {
        group.style.animationDelay = `${index * 0.1}s`;
        group.classList.add('fade-in-up');
    });
    
    // Add focus effects
    inputs.forEach(input => {
        input.addEventListener('focus', function() {
            this.parentNode.classList.add('focused');
        });
        
        input.addEventListener('blur', function() {
            this.parentNode.classList.remove('focused');
        });
    });
    
    // Auto-hide success message after 5 seconds
    const successAlert = document.querySelector('.alert-success');
    if (successAlert) {
        setTimeout(() => {
            if (successAlert.parentNode) {
                successAlert.remove();
            }
        }, 5000);
    }
});

// Add CSS for animations
const style = document.createElement('style');
style.textContent = `
    .fade-in-up {
        animation: fadeInUp 0.6s ease-out forwards;
        opacity: 0;
        transform: translateY(20px);
    }
    
    @keyframes fadeInUp {
        to {
            opacity: 1;
            transform: translateY(0);
        }
    }
    
    .form-group.focused .form-label {
        color: #2563eb;
        transform: translateY(-2px);
    }
    
    .password-field {
        position: relative;
    }
    
    .password-toggle {
        position: absolute;
        right: 1rem;
        top: 50%;
        transform: translateY(-50%);
        background: none;
        border: none;
        color: #9ca3af;
        cursor: pointer;
        padding: 0.5rem;
        border-radius: 50%;
        transition: all 0.3s ease;
        z-index: 10;
    }
    
    .password-toggle:hover {
        background: rgba(37, 99, 235, 0.1);
        color: #2563eb;
    }
    
    .password-toggle:focus {
        outline: none;
        box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.1);
    }
`;
document.head.appendChild(style); 