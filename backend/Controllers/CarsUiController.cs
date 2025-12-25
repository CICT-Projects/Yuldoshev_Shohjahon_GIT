using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("/cars-ui")]
    public class CarsUiController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            var html = @"<!doctype html>
<html>
<head>
  <meta charset='utf-8'> 
  <title>Cars UI</title>
  <style>body{font-family:Arial,Helvetica,sans-serif}table{border-collapse:collapse;width:100%;margin-top:8px}th,td{border:1px solid #ccc;padding:6px;text-align:left}form>label{display:block;margin:6px 0}</style>
</head>
<body>
  <h1>Cars UI</h1>

  <section>
    <form id='carForm'>
      <input type='hidden' id='carId' />
      <label>Make: <input id='make' required /></label>
      <label>Model: <input id='model' required /></label>
      <label>Year: <input id='year' type='number' /></label>
      <button type='submit'>Save</button>
      <button type='button' id='resetBtn'>Reset</button>
    </form>
  </section>

  <section>
    <button id='refresh'>Refresh list</button>
    <table id='carsTable'>
      <thead><tr><th>Id</th><th>Make</th><th>Model</th><th>Year</th><th>Actions</th></tr></thead>
      <tbody></tbody>
    </table>
  </section>

  <script>
    const api = '/api/cars';

    async function fetchCars(){
      const res = await fetch(api);
      const data = await res.json();
      const tbody = document.querySelector('#carsTable tbody');
      tbody.innerHTML = '';
      data.forEach(c => {
        const tr = document.createElement('tr');
        tr.innerHTML = `
          <td>${c.id}</td>
          <td>${c.make}</td>
          <td>${c.model}</td>
          <td>${c.year ?? ''}</td>
          <td>
            <button data-id='${c.id}' class='edit'>Edit</button>
            <button data-id='${c.id}' class='del'>Delete</button>
          </td>`;
        tbody.appendChild(tr);
      });
      // wire buttons
      document.querySelectorAll('button.edit').forEach(b => b.onclick = e => {
        const id = e.target.dataset.id;
        const row = e.target.closest('tr');
        document.getElementById('carId').value = id;
        document.getElementById('make').value = row.children[1].textContent;
        document.getElementById('model').value = row.children[2].textContent;
        document.getElementById('year').value = row.children[3].textContent;
      });
      document.querySelectorAll('button.del').forEach(b => b.onclick = async e => {
        if (!confirm('Delete this car?')) return;
        const id = e.target.dataset.id;
        await fetch(`${api}/${id}`, { method: 'DELETE' });
        fetchCars();
      });
    }

    async function createCar(c){
      await fetch(api, { method: 'POST', headers: {'Content-Type':'application/json'}, body: JSON.stringify(c) });
    }

    async function updateCar(id, c){
      await fetch(`${api}/${id}`, { method: 'PUT', headers: {'Content-Type':'application/json'}, body: JSON.stringify(c) });
    }

    document.getElementById('carForm').addEventListener('submit', async (e) => {
      e.preventDefault();
      const id = document.getElementById('carId').value;
      const car = { id: id ? parseInt(id) : 0, make: document.getElementById('make').value, model: document.getElementById('model').value, year: parseInt(document.getElementById('year').value || '0') };
      if (id) await updateCar(id, car); else await createCar(car);
      resetForm();
      fetchCars();
    });

    document.getElementById('resetBtn').addEventListener('click', resetForm);
    document.getElementById('refresh').addEventListener('click', fetchCars);

    function resetForm(){ document.getElementById('carId').value = ''; document.getElementById('make').value = ''; document.getElementById('model').value = ''; document.getElementById('year').value = ''; }

    fetchCars();
  </script>
</body>
</html>";

            return Content(html, "text/html");
        }
    }
}
