const beanVarietyUrl = "https://localhost:5001/api/beanvariety/";
const coffeeUrl = "https://localhost:5001/api/coffee/";

const button = document.querySelector("#run-button");
button.addEventListener("click", async () => {
    try {
        const beanVarieties = await getAllBeanVarieties();
        const coffee = await getAllCoffee();

        // Combine bean varieties and coffee data for display
        const combinedData = coffee.map(coffeeItem => ({
            ...coffeeItem,
            beanVariety: beanVarieties.find(variety => variety.id === coffeeItem.beanVarietyId)
        }));

        // Create HTML elements for the coffee list
        const coffeeList = document.createElement("ul");
        combinedData.forEach(coffeeItem => {
            const coffeeListItem = document.createElement("li");
            coffeeListItem.textContent = `${coffeeItem.title} (${coffeeItem.beanVariety.name})`;
            coffeeList.appendChild(coffeeListItem);
        });

        // Append the coffee list to the HTML
        document.querySelector(".allTheCoffee").appendChild(coffeeList);
    } catch (error) {
        console.error("Error fetching data:", error);
    }
});

async function getAllBeanVarieties() {
    const response = await fetch(beanVarietyUrl);
    if (!response.ok) {
        throw new Error(`Failed to fetch bean varieties: ${response.status}`);
    }
    return response.json();
}

async function getAllCoffee() {
    const response = await fetch(coffeeUrl);
    if (!response.ok) {
        throw new Error(`Failed to fetch coffee: ${response.status}`);
    }
    return response.json();
}
