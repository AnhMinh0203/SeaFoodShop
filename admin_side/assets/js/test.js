// axios.get(`https://localhost:7018/api/Type/GetTypes?token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiI2NGEwYjNhYi0xNmEzLTRiNzgtOGZjOC1hNmVkNzRlYzc4ZjIiLCJuYmYiOjE3MTQ2MTU3OTksImV4cCI6MTcxNDcwMjE5OSwiaWF0IjoxNzE0NjE1Nzk5fQ.9EynUhwi_YgkXmXvY5BKGwDSurzOnFegOIx5CQ7iA1Q`)
//     .then(response => {
//         // Truy cập vào dữ liệu trong [[promiseResult]]
//         console.log(response.data)

//     })
//     .catch(error => {
//         console.error('Error fetching data:', error);
//     });

// var nameType = 'Chả'
// axios.post(`https://localhost:7018/api/SeaFood/SearchSeafoodByType?nameSeaFood=${nameType}&pageIndex=1&pageSize=2`)
//     .then(response => {
//             // Truy cập vào dữ liệu trong [[promiseResult]]
//             console.log(response.data)
    
//         })
//         .catch(error => {
//             console.error('Error fetching data:', error);
//         });

async function fetchSearchProductByTypeData (nameType, pageIndex, pageSize){
    return await axios.post(`https://localhost:7018/api/SeaFood/SearchSeafoodByType?nameSeaFood=${nameType}&pageIndex=1&pageSize=2`)
}

fetchSearchProductByTypeData('Cá',1,5)
.then (res=>{
    console.log(res)
})