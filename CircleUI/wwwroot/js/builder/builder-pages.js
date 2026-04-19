(function(b) {
    const projectId = () => b.projectId;

    // ── Rename ──────────────────────────────────────────────────────────
    let renamePageId = null;
    const renameModal = () => bootstrap.Modal.getOrCreateInstance(document.getElementById('renamePageModal'));

    document.addEventListener('click', e => {
        const btn = e.target.closest('.rename-page-btn');
        if (!btn) return;
        e.preventDefault();
        e.stopPropagation();
        renamePageId = btn.dataset.pageId;
        document.getElementById('renamePageInput').value = btn.dataset.pageTitle;
        renameModal().show();
        setTimeout(() => document.getElementById('renamePageInput').select(), 300);
    });

    document.getElementById('renamePageSaveBtn').addEventListener('click', async () => {
        const title = document.getElementById('renamePageInput').value.trim();
        if (!title || !renamePageId) return;

        const token = b.getAntiForgeryToken();
        const body = `pageId=${encodeURIComponent(renamePageId)}&projectId=${encodeURIComponent(projectId())}&title=${encodeURIComponent(title)}`;
        const res = await fetch('/Page/Rename', {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'RequestVerificationToken': token },
            body
        });
        const data = await res.json();
        if (data.success) {
            // Update tab button text
            const tabBtn = document.getElementById(`tab-${renamePageId}`);
            if (tabBtn) tabBtn.textContent = data.title;
            // Update rename button's stored title for next open
            const renameBtn = document.querySelector(`.rename-page-btn[data-page-id="${renamePageId}"]`);
            if (renameBtn) renameBtn.dataset.pageTitle = data.title;
            renameModal().hide();
        }
    });

    document.getElementById('renamePageInput').addEventListener('keydown', e => {
        if (e.key === 'Enter') document.getElementById('renamePageSaveBtn').click();
    });

    // ── Drag-to-reorder tabs ─────────────────────────────────────────────
    let dragSrc = null;

    function getTabList() {
        return document.getElementById('pageTabs');
    }

    document.addEventListener('dragstart', e => {
        const li = e.target.closest('.page-tab-item');
        if (!li) return;
        dragSrc = li;
        e.dataTransfer.effectAllowed = 'move';
        setTimeout(() => li.classList.add('opacity-50'), 0);
    });

    document.addEventListener('dragend', e => {
        const li = e.target.closest('.page-tab-item');
        if (li) li.classList.remove('opacity-50');
        document.querySelectorAll('.page-tab-item').forEach(el => el.classList.remove('drag-over-tab'));
        dragSrc = null;
    });

    document.addEventListener('dragover', e => {
        const li = e.target.closest('.page-tab-item');
        if (!li || li === dragSrc) return;
        e.preventDefault();
        e.dataTransfer.dropEffect = 'move';
        document.querySelectorAll('.page-tab-item').forEach(el => el.classList.remove('drag-over-tab'));
        li.classList.add('drag-over-tab');
    });

    document.addEventListener('dragleave', e => {
        const li = e.target.closest('.page-tab-item');
        if (li) li.classList.remove('drag-over-tab');
    });

    document.addEventListener('drop', async e => {
        const target = e.target.closest('.page-tab-item');
        if (!target || !dragSrc || target === dragSrc) return;
        e.preventDefault();
        target.classList.remove('drag-over-tab');

        const tabList = getTabList();
        const items = [...tabList.querySelectorAll('.page-tab-item')];
        const srcIdx = items.indexOf(dragSrc);
        const tgtIdx = items.indexOf(target);

        // Reorder DOM
        if (srcIdx < tgtIdx) {
            target.after(dragSrc);
        } else {
            target.before(dragSrc);
        }

        // Persist new order
        const newOrder = [...tabList.querySelectorAll('.page-tab-item')].map(li => li.dataset.pageId);
        const token = b.getAntiForgeryToken();
        await fetch(`/Page/Reorder?projectId=${encodeURIComponent(projectId())}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(newOrder)
        });
    });
})(window.builder);
