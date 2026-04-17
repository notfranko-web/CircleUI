(function(b) {
    b.openLinkModal = function(el) {
        b.activeLinkElement = el;
        const textInput = document.getElementById('linkText');
        const hrefInput = document.getElementById('linkHref');
        const pageSelect = document.getElementById('linkPage');
        const targetCheck = document.getElementById('linkTargetBlank');
        const removeBtn = document.getElementById('removeLinkBtn');

        textInput.value = el.innerText.trim();

        if (el.tagName === 'A') {
            const href = el.getAttribute('href') || '';
            hrefInput.value = href;
            targetCheck.checked = el.getAttribute('target') === '_blank';
            removeBtn.classList.remove('d-none');
            
            pageSelect.value = '';
            Array.from(pageSelect.options).forEach(opt => {
                if (opt.value && opt.value === href) pageSelect.value = opt.value;
            });
        } else {
            hrefInput.value = '';
            pageSelect.value = '';
            targetCheck.checked = false;
            removeBtn.classList.add('d-none');
        }

        b.getLinkModal().show();
    };

    b.wireLinksInContainer = function(containerEl) {
        containerEl.querySelectorAll('a, button.btn, .btn').forEach(function(el) {
            if (el.closest('.carousel-control-prev') || el.closest('.carousel-control-next') || el.classList.contains('remove-component-btn')) return;
            
            el.addEventListener('click', function(e) {
                e.preventDefault();
                e.stopPropagation();
                b.openLinkModal(this);
            }, true);
        });
    };
    
    b.openLightbox = function(img, isComponentImg) {
        b.activeImageElement = isComponentImg ? img : null;

        const lbImg    = document.getElementById('lightboxImage');
        const range    = document.getElementById('imageWidthRange');
        const rangeVal = document.getElementById('imageWidthVal');
        const saveBtn  = document.getElementById('saveResizeBtn');
        const chgBtn   = document.getElementById('changeImageBtn');

        lbImg.src = img.src;
        document.getElementById('lightboxCaption').textContent = img.title || img.alt || 'Image Preview';
        lbImg.style.maxHeight  = '80vh';
        lbImg.style.height     = 'auto';
        lbImg.style.width      = 'auto';
        lbImg.style.objectFit  = 'contain';
        lbImg.classList.add('d-block', 'mx-auto');

        if (isComponentImg) {
            const w = parseInt(img.style.maxWidth || img.style.width) || 50;
            range.value = w;
            rangeVal.textContent = w + '%';
            lbImg.style.maxWidth = w + '%';
            saveBtn.classList.remove('d-none');
            chgBtn.classList.remove('d-none');
        } else {
            lbImg.style.maxWidth = '100%';
            saveBtn.classList.add('d-none');
            chgBtn.classList.add('d-none');
        }

        b.getLightboxModal().show();
    };

    b.wireImagesInContainer = function(containerEl) {
        containerEl.querySelectorAll('img').forEach(function(img) {
            img.contentEditable = 'false';
            img.setAttribute('draggable', 'false');
            img.style.cursor = 'pointer';
            img.onclick = function(e) {
                e.preventDefault();
                e.stopPropagation();
                b.openLightbox(this, true);
            };
        });
    };
    
    window.addEventListener('DOMContentLoaded', () => {
        const linkPageSelect = document.getElementById('linkPage');
        if (linkPageSelect) {
            linkPageSelect.addEventListener('change', function() {
                if (this.value) {
                    document.getElementById('linkHref').value = this.value;
                }
            });
        }
        
        const applyLinkBtn = document.getElementById('applyLinkBtn');
        if (applyLinkBtn) {
            applyLinkBtn.addEventListener('click', function() {
                if (!b.activeLinkElement) return;
                const text = document.getElementById('linkText').value;
                const href = document.getElementById('linkHref').value;
                const target = document.getElementById('linkTargetBlank').checked ? '_blank' : null;

                let targetEl = b.activeLinkElement;
                targetEl.innerText = text;

                if (targetEl.tagName !== 'A') {
                    const a = document.createElement('a');
                    a.href = href || '#';
                    if (target) a.target = target;
                    a.innerText = text;
                    
                    if (targetEl.classList.contains('btn')) {
                        a.className = targetEl.className;
                        targetEl.parentNode.replaceChild(a, targetEl);
                        targetEl = a;
                    } else {
                        targetEl.parentNode.insertBefore(a, targetEl);
                        a.appendChild(targetEl);
                        targetEl = a;
                    }
                } else {
                    targetEl.setAttribute('href', href || '#');
                    if (target) {
                        targetEl.setAttribute('target', target);
                    } else {
                        targetEl.removeAttribute('target');
                    }
                }

                b.saveComponentContent(targetEl.closest('.component-editable'));
                b.getLinkModal().hide();
            });
        }
        
        const removeLinkBtn = document.getElementById('removeLinkBtn');
        if (removeLinkBtn) {
            removeLinkBtn.addEventListener('click', function() {
                if (!b.activeLinkElement || b.activeLinkElement.tagName !== 'A') return;
                
                const parent = b.activeLinkElement.parentNode;
                const children = Array.from(b.activeLinkElement.childNodes);
                
                if (b.activeLinkElement.classList.contains('btn')) {
                     const btn = document.createElement('button');
                     btn.type = 'button';
                     btn.className = b.activeLinkElement.className;
                     btn.innerHTML = b.activeLinkElement.innerHTML;
                     parent.replaceChild(btn, b.activeLinkElement);
                } else {
                     children.forEach(child => parent.insertBefore(child, b.activeLinkElement));
                     parent.removeChild(b.activeLinkElement);
                }

                b.saveComponentContent(parent.closest('.component-editable'));
                b.getLinkModal().hide();
            });
        }
        
        const widthRange = document.getElementById('imageWidthRange');
        if (widthRange) {
            widthRange.addEventListener('input', function() {
                document.getElementById('lightboxImage').style.maxWidth = this.value + '%';
                document.getElementById('imageWidthVal').textContent = this.value + '%';
            });
        }
        
        const saveResizeBtn = document.getElementById('saveResizeBtn');
        if (saveResizeBtn) {
            saveResizeBtn.addEventListener('click', function() {
                if (!b.activeImageElement) return;
                const w = document.getElementById('imageWidthRange').value + '%';
                b.activeImageElement.style.maxWidth   = w;
                b.activeImageElement.style.width      = 'auto';
                b.activeImageElement.style.height     = 'auto';
                b.activeImageElement.style.objectFit  = 'contain';
                b.activeImageElement.classList.add('d-block', 'mx-auto');
                b.saveComponentContent(b.activeImageElement.closest('.component-editable'));
                b.getLightboxModal().hide();
            });
        }
        
        const changeImageBtn = document.getElementById('changeImageBtn');
        if (changeImageBtn) {
            changeImageBtn.addEventListener('click', function() {
                b.getLightboxModal().hide();
                b.getImageSelectModal().show();
            });
        }
        
        const lightboxImg = document.getElementById('lightboxImage');
        if (lightboxImg) {
            lightboxImg.addEventListener('click', function() {
                b.getLightboxModal().hide();
            });
        }
    });
})(window.builder);