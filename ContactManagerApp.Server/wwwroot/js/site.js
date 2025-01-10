const table = document.getElementById('contactsTable');
const searchInput = document.getElementById('searchInput');
const deleteButtons = document.querySelectorAll('.delete-btn');

// Delete Functionality
deleteButtons.forEach(deleteBtn => {
    deleteBtn.addEventListener('click', async (event) => {
        const row = event.target.closest('tr'); // Get the closest row
        const contactId = row.getAttribute('data-id'); // Get the contact ID of the row

        // Send DELETE request to server
        const response = await fetch(`/contacts/${contactId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            row.remove(); // If the response is ok, remove the row from the table
            alert('Contact deleted successfully');
        } else {
            alert('Failed to delete contact');
        }
    });
});

// Edit Functionality
table.addEventListener('click', async (event) => {
    if (event.target.classList.contains('save-btn')) {
        const row = event.target.closest('tr');
        const contactId = row.getAttribute('data-id'); // Get the contact ID of the row
        const cells = row.querySelectorAll('td'); // Get all cells in the row

        // Create updated contact object
        const updatedContact = {
            Id: contactId,
            Name: cells[0].innerText,
            DateOfBirth: new Date(cells[1].innerText),
            Married: cells[2].innerText.toLowerCase() === 'true',
            Phone: cells[3].innerText,
            Salary: parseFloat(cells[4].innerText)
        };

        // Send PUT request to server
        const response = await fetch(`/Contacts`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedContact)
        });

        if (response.ok) {
            alert('Contact updated successfully');
        } else {
            alert('Failed to update contact');
        }
    }

});

// Sort Functionality
table.querySelectorAll('th').forEach((header, index) => {
    header.addEventListener('click', () => {
        const rows = Array.from(table.querySelectorAll('tbody tr')); // Get all rows
        const isAscending = header.classList.toggle('ascending'); // Toggle the class and get the current sorting order

        // Sort rows based on the cell value
        rows.sort((a, b) => {
            const cellA = a.children[index].innerText.trim();
            const cellB = b.children[index].innerText.trim();

            // Handle number sorting
            if (!isNaN(cellA) && !isNaN(cellB)) {
                return isAscending ? cellA - cellB : cellB - cellA; // Convert to number and sort
            }

            // Handle text sorting
            return isAscending ? cellA.localeCompare(cellB) : cellB.localeCompare(cellA);
        });

        // Re-append sorted rows
        rows.forEach(row => table.querySelector('tbody').appendChild(row));
    });
});

// Filter Functionality
searchInput.addEventListener('input', () => {
    const query = searchInput.value.toLowerCase();
    table.querySelectorAll('tbody tr').forEach(row => {
        // Check if any cell in the row contains the search query
        const isVisible = Array.from(row.children).some(cell =>
            cell.innerText.toLowerCase().includes(query) // Check if the cell contains the query
        );
        row.style.display = isVisible ? '' : 'none'; // Show or hide the row
    });
});