function sortTable(tableId, columnIndex, th) {

    var ascending = true;
    if (th.classList.contains("desc")) {
        ascending = true;
    } else if (th.classList.contains("asc")) {
        ascending = false;
    }

    var table, rows, switching, i, x, y, shouldSwitch;
    table = document.getElementById(tableId);
    switching = true;

    while (switching) {
        switching = false;
        rows = table.rows;

        for (i = 1; i < (rows.length - 1); i++) {
            shouldSwitch = false;

            x = rows[i].getElementsByTagName("td")[columnIndex];
            y = rows[i + 1].getElementsByTagName("td")[columnIndex];

            if (ascending) {
                if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            } else {
                if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                    shouldSwitch = true;
                    break;
                }
            }
        }

        if (shouldSwitch) {
            rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
            switching = true;
        }
    }

    var headers = table.getElementsByTagName("th");
    for (let j = 0; j < headers.length - 1; j++) {
        if (headers[j] !== th) {
            headers[j].classList.remove("asc", "desc");
            headers[j].querySelector(".sort-arrow").textContent = ""; // Clear arrow
        }
    }

    th.classList.toggle("asc", ascending);
    th.classList.toggle("desc", !ascending);
    th.querySelector(".sort-arrow").textContent = ascending ? " ▲" : " ▼";
}