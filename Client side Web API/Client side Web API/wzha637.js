    const home = () => {
    document.getElementById("home").style.display = "block";
    document.getElementById("staff").style.display = "none";
    document.getElementById("shop").style.display = "none";
    document.getElementById("register").style.display = "none";
    document.getElementById("guestBook").style.display = "none";
    document.getElementById("login").style.display = "none";
}

const staff = () => {
    document.getElementById("home").style.display = "none";
    document.getElementById("staff").style.display = "block";
    document.getElementById("shop").style.display = "none";
    document.getElementById("register").style.display = "none";
    document.getElementById("guestBook").style.display = "none";
    document.getElementById("login").style.display = "none";
}

const shop = () => {
    document.getElementById("home").style.display = "none";
    document.getElementById("staff").style.display = "none";
    document.getElementById("shop").style.display = "block";
    document.getElementById("register").style.display = "none";
    document.getElementById("guestBook").style.display = "none";
    document.getElementById("login").style.display = "none";
}

const register = () => {
    document.getElementById("home").style.display = "none";
    document.getElementById("staff").style.display = "none";
    document.getElementById("shop").style.display = "none";
    document.getElementById("register").style.display = "block";
    document.getElementById("guestBook").style.display = "none";
    document.getElementById("login").style.display = "none";
}

const guestBook = () => {
    document.getElementById("home").style.display = "none";
    document.getElementById("staff").style.display = "none";
    document.getElementById("shop").style.display = "none";
    document.getElementById("register").style.display = "none";
    document.getElementById("guestBook").style.display = "block";
    document.getElementById("login").style.display = "none";
}
const login = () => {
    document.getElementById("home").style.display = "none";
    document.getElementById("staff").style.display = "none";
    document.getElementById("shop").style.display = "none";
    document.getElementById("register").style.display = "none";
    document.getElementById("guestBook").style.display = "none";
    document.getElementById("login").style.display = "block";
}

const getItems = () => {
    const fetchPromises = fetch("http://localhost:5000/api/GetItems")
    const streamPromises = fetchPromises.then((response) => response.json());
    streamPromises.then((data) => showItems(data));

}
const showItems = (items) => {
    let htmlString="<tr><td><h3>Item Picture</h3></td><td><h3>Description</h3></td></tr>"
    const showItem = (item) => {
        htmlString += `<tr>
                    <td><img src="http://localhost:5000/api/GetItemPhoto/${item.id}" width="280" height="300"></td>
                    <td>
                    <h3>${item.name}</h3>
                    <p>${item.description}</p>
                    <p>$${item.price}</p>
                    <button class="button" onclick="buyItem(${item.id})">Buy</button>
                    </td>
                    </tr>`
    }
    items.forEach(showItem);
    const table = document.getElementById("itemTable");
    table.innerHTML = htmlString;
}

const getAllStaff = () => {
    const fetchPromises = fetch("http://localhost:5000/api/GetAllStaff")
    const streamPromises = fetchPromises.then((response) => response.json());
    streamPromises.then((data) => showStaffs(data));
    }

const showStaffs = (staffs) => {
    let htmlString="<tr><td><h3>Staff Picture</h3></td><td><h3>Description</h3></td></tr>"
    const showStaff = (staff) => {
        const fetchPromises = fetch(`http://localhost:5000/api/GetCard/${staff.id}`, {
            headers: {
                'content-type': 'text/vcard'
            }
        });
        const streamPromises = fetchPromises.then((response) => response.text());
        streamPromises.then((data) => parse(data));

        const parse = (data) => {
            let res = {};
            data.split('\n').forEach((line) => {
                let temp = line.split(':')
                res[temp[0]] = temp[1]
            });

            htmlString += `<tr>
                    <td><img src="http://localhost:5000/api/GetStaffPhoto/${staff.id}" width="280" height="300"></td>
                    <td><a href="http://localhost:5000/api/GetCard/${staff.id}">Download the card </a><br>
                       <a href="${res['FN']}">${res['FN']}</a><br>
                       <a href="${res['EMAIL;TYPE=work']}">${res['EMAIL;TYPE=work']}</a>
                    </td>
                    </tr>`
            document.getElementById("staffTable").innerHTML = htmlString
        };
    }
    staffs.forEach(showStaff);
    const table = document.getElementById("staffTable");
    table.innerHTML = htmlString;
}

const registerUser = () => {
    let username = document.getElementById("nameInput").value;
    let password = document.getElementById("passInput").value;
    let address = document.getElementById("addrInput").value;
    let user = { 'username': username, 'password': password, 'address': address }

    const result = fetch('http://localhost:5000/api/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    }).then((response) => response.text()).then((data) => { document.getElementById("result").innerText = data;
    })
}

const getComment = () => {
    const result = fetch("http://localhost:5000/api/GetComments", {
            headers: {
                'content-type': 'text/html'
            }
        });
        const streamPromises = result.then((response) => response.text());
        streamPromises.then((data) => document.getElementById("commentBoard").innerHTML = data);
}

const writeComment = () => {
    let username = document.getElementById("guestName").value;
    let comment = document.getElementById("comment").value;
    let commentObj = { 'comment': comment, 'name': username }

    const feedback = fetch('http://localhost:5000/api/writecomment', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(commentObj)

    }).then(() => getComment());
}

let state = false;
let username = ""
const userLogin = () => {
    username = document.getElementById("loginName").value;
    password = document.getElementById("loginPassword").value;
    const result = fetch('http://localhost:5000/api/GetVersionA', {
        method: 'GET',
        headers: {
            'Content-Type': 'text/plain; charset=utf-8',
            'Authorization': 'Basic ' + btoa(`${username}:${password}`)
        }
    }).then(response => response.text());
    result.then((data) => {
        if (data == 'v1') {
            state = true;
            checkLogin();
            alert('Login Success!')
        } else {
            state = false;
            alert('Incorrect username or password!')
        }
    })
}

const checkLogin = () => {
    if(state == true) {
        document.getElementById("checkLogin").innerHTML = `<p>Welcome ${username} <button class="button3" onclick="logout();">Sign Out</button></p>`;
    } else {
        document.getElementById("checkLogin").innerHTML = `<p>Click here to login <button class="button2" onclick="login();">login</button></p>`;
    }
}


const logout = () => {
    state = false;
    checkLogin();
}

const buyItem = (id) => {
    if(state === false) {
        alert("Login Required!")
        login();
    } else {
        const buy = fetch(`http://localhost:5000/api/PurchaseSingleItem/${id}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'text/plain; charset=utf-8',
                'Authorization': 'Basic ' + btoa(`${username}:${password}`)
            }
        }).then(response => response.json());
        buy.then((data) => popWindow(data))
    }
}


const popWindow = (data) => {
        document.getElementById("window").style.display = 'block'
        document.getElementById("window").innerHTML = `<p>Thank you user ${data['userName']}!</p><p>You have bought ${data['productID']}</p><br><button onclick="closeWindow();">Close</button>`
}

const closeWindow = () => {
    document.getElementById("window").style.display = 'none'
}


const searchItem = () => {
    let name = document.getElementById('itemSearch').value;
    const fetchPromises = fetch(`http://localhost:5000/api/GetItems/${name}`)
        .then((res) => res.json());
    fetchPromises.then((data) => showItems(data));
}

const load = () => {
    home();
    getItems();
    getAllStaff();
    getComment();
    checkLogin();

}
window.onload = load;
