document.addEventListener('DOMContentLoaded', () => {
    const productList = document.getElementById('the-products');
    const productPickForm = document.getElementById('product-form');
    const findSlotsBtn = document.getElementById('get-delivery-slots');
    const slotsSection = document.getElementById('time-slots-section');
    const slotsDiv = document.getElementById('slot-display-area');
    const noSlotsMessage = document.getElementById('no-slots-found');
    const loadSpinner = document.getElementById('loader');
    const errDisplay = document.getElementById('error-msg');
    const confirmOrderPanel = document.getElementById('confirm-order-section');
    const finalizeOrderBtn = document.getElementById('place-order-button');

    let allProductsData = []; 
    let selectedIds = [];
    let currentDeliveryTimes = [];
    let chosenDeliverySlot = null;

    const API_BASE = ' https://grocerystoretask.tryasp.net/api'; 

    const ShowElement = (element) => element.classList.remove('hiden');
    const HideElement = (element) => element.classList.add('hiden');
    const setLoading = (isBusy) => {
        if (isBusy) {
            ShowElement(loadSpinner);
            findSlotsBtn.disabled = true;
            productList.disabled = true;
            finalizeOrderBtn.disabled = true;
        } else {
            HideElement(loadSpinner);
            findSlotsBtn.disabled = false;
            productList.disabled = false;
            finalizeOrderBtn.disabled = false;
        }
    };
    const showError = (message) => {
        errDisplay.textContent = message;
        ShowElement(errDisplay);
    };
    const clearError = () => HideElement(errDisplay);

    const GetProducts = async () => {
        setLoading(true);
        clearError();
        try {
            const response = await fetch(`${API_BASE}/Product`);
            if (!response.ok) throw new Error(`HTTP ${response.status}`);
            allProductsData = await response.json();
            putProductsInSelect(allProductsData);
        } catch (e) {
            console.error('Failed products:', e); 
            showError(`Couldn't get product list: ${e.message}`);
        } 
            setLoading(false);
       
    };

    const putProductsInSelect = (products) => {
        productList.innerHTML = ''; 
        if (products.length === 0) {
            productList.appendChild(new Option('No products available', '', true, true)); 
            findSlotsBtn.disabled = true;
            return;
        }

        products.forEach(p => {
            const opt = document.createElement('option');
            opt.value = p.id;
            opt.textContent = `${p.name} (${p.category})`;
            productList.appendChild(opt);
        });
        findSlotsBtn.disabled = false;
    };

    productPickForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        selectedIds = Array.from(productList.selectedOptions).map(opt => opt.value);

        if (selectedIds.length === 0) {
            showError('Please select some products before continuing.');
            HideElement(slotsSection);
            return;
        }

        clearError();
        await fetchDeliverySchedule(selectedIds);
    });

    const fetchDeliverySchedule = async (productsIds) => {
        setLoading(true);
        HideElement(slotsSection);
        HideElement(noSlotsMessage);
        slotsDiv.innerHTML = '';
        chosenDeliverySlot = null;
        HideElement(confirmOrderPanel);

         const queryParams = new URLSearchParams();
        productsIds.forEach(id => queryParams.append('productIds', id));
        queryParams.append('orderDate', new Date().toLocaleString())
        const queryString = queryParams.toString();
        
        try {
            const response = await fetch(`${API_BASE}/Schedule?${queryString}`, { 
                method: 'GET',
            });

            if (!response.ok) throw new Error(`HTTP status: ${response.status}`); 
            currentDeliveryTimes = await response.json();
            displaySlots(currentDeliveryTimes);

        } catch (e) {
            console.error('Error with schedule:', e);
            showError(`Problem fetching schedule: ${e.message}`);
        } 
            setLoading(false);
        
    };
   

    const displaySlots = (slots) => {
        ShowElement(slotsSection);
        if (slots.length === 0) {
            ShowElement(noSlotsMessage);
            HideElement(slotsDiv);
            return;
        }

        HideElement(noSlotsMessage);
        ShowElement(slotsDiv);

        slots.forEach((slot, i) => { 
            const slotItem = document.createElement('div');
            slotItem.classList.add('slot-entry');
            if (slot.isGreen) {
                slotItem.classList.add('green');
            }
            slotItem.dataset.startDate = slot.startDate;
            slotItem.dataset.endDate = slot.endDate;
            slotItem.dataset.isGreen = slot.isGreen;

            const startFormatted = new Date(slot.startDate).toLocaleString();
            const endFormatted = new Date(slot.endDate).toLocaleString();

            slotItem.innerHTML = `
                <div>
                    <strong>Starts:</strong> ${startFormatted}<br>
                    <strong>Ends:</strong> ${endFormatted}
                </div>
                <span class="slot-tag ${slot.isGreen ? 'green-tag' : 'normal-tag'}">
                    ${slot.isGreen ? 'GREENSlot' : 'NormalSlot'}
                </span>
            `;
            slotsDiv.appendChild(slotItem);

            slotItem.addEventListener('click', () => {
                const prevSelected = slotsDiv.querySelector('.slot-entry.chosen');
                if (prevSelected) {
                    prevSelected.classList.remove('chosen');
                }
                slotItem.classList.add('chosen');
                chosenDeliverySlot = slot; 
                ShowElement(confirmOrderPanel);
            });
        });
    };

    finalizeOrderBtn.addEventListener('click', async () => {
        if (!chosenDeliverySlot || selectedIds.length === 0) { 
            showError('Need to pick a slot AND products.');
            return;
        }

        setLoading(true);
        clearError();

        const cartPayload = {
            startDate: chosenDeliverySlot.startDate,
            isGreen: chosenDeliverySlot.isGreen,
            cartProductsIds: selectedIds,
        };

        try {
            const response = await fetch(`${API_BASE}/Cart`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(cartPayload),
            });

            if (response.status === 204) {
                alert('Order placed successfully!'); 
                productList.selectedIndex = -1; 
                selectedIds = [];
                currentDeliveryTimes = [];
                chosenDeliverySlot = null;
                HideElement(slotsSection);
                HideElement(confirmOrderPanel);
                slotsDiv.innerHTML = '';
            } else if (response.status === 400) {
                const apiError = await response.text();
                showError(`Order failed: ${apiError || 'Bad request from server'}`);
            } else {
                throw new Error(`API response: ${response.status}`);
            }
        } catch (e) {
            console.error('Cart confirm error:', e);
            showError(`Could not finalize order: ${e.message}`);
        } 
            setLoading(false);
        
    });

    
    GetProducts();
});