document.addEventListener('DOMContentLoaded', function () {
    const textarea = document.getElementById('contentArea');
    const counter = document.getElementById('wordCount');
    const imageInput = document.getElementById('imageInput');
    const imagePreview = document.getElementById('imagePreview');
    const previewImg = document.getElementById('previewImg');
    const removePreviewBtn = document.getElementById('removePreview');
    const removeFlag = document.getElementById('removeImageFlag');
    const currentImage = document.getElementById('currentImage');
    const removeCurrentBtn = document.getElementById('removeCurrentImage');
    const form = document.querySelector('form');
    let formChanged = false;

    function updateWordCount() {
        if (!(textarea instanceof HTMLTextAreaElement) || !counter) return;
        const text = textarea.value.trim();
        const count = text.length > 0 ? text.split(/\s+/).length : 0;
        counter.textContent = count + ' words';
    }

    function showPreview(file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            if (e.target && typeof e.target.result === 'string' && previewImg) {
                previewImg.setAttribute('src', e.target.result);
            }
            if (imagePreview) imagePreview.style.display = 'block';
        };
        reader.readAsDataURL(file);
    }

    function hidePreview() {
        if (imagePreview) imagePreview.style.display = 'none';
        if (previewImg) previewImg.setAttribute('src', 'data:,');
    }

    function clearFileInput() {
        if (imageInput instanceof HTMLInputElement) imageInput.value = '';
    }

    if (textarea instanceof HTMLTextAreaElement) {
        textarea.addEventListener('input', updateWordCount);
        updateWordCount();
    }

    if (imageInput instanceof HTMLInputElement) {
        imageInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                showPreview(this.files[0]);
                if (currentImage) currentImage.style.display = 'none';
                if (removeFlag) removeFlag.setAttribute('value', 'false');
            } else {
                hidePreview();
            }
        });
    }

    if (removePreviewBtn) {
        removePreviewBtn.addEventListener('click', function () {
            clearFileInput();
            hidePreview();
            if (currentImage) currentImage.style.display = '';
        });
    }

    if (removeCurrentBtn && currentImage) {
        removeCurrentBtn.addEventListener('click', function () {
            currentImage.style.display = 'none';
            if (removeFlag) removeFlag.setAttribute('value', 'true');
        });
    }

    if (form) {
        form.addEventListener('input', function () {
            formChanged = true;
        });

        form.addEventListener('submit', function () {
            formChanged = false;
        });

        window.addEventListener('beforeunload', function (e) {
            if (formChanged) e.preventDefault();
        });
    }
});