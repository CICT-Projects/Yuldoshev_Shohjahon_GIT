import { useEffect, useState } from 'react'
import './App.css'

const API_BASE = import.meta.env.VITE_API_BASE ?? 'http://localhost:5161'

function App() {
  const [view, setView] = useState('parts')
  const [data, setData] = useState([])
  const [error, setError] = useState(null)
  const [form, setForm] = useState({})
  const [loading, setLoading] = useState(false)

  const definitions = {
    parts: { title: 'Parts', endpoint: '/api/parts', fields: [ ['name','Name'], ['category','Category'], ['price','Price'] ] },
    cars: { title: 'Cars', endpoint: '/api/cars', fields: [ ['make','Make'], ['model','Model'], ['year','Year'] ] },
    motors: { title: 'Motors', endpoint: '/api/motors', fields: [ ['name','Name'], ['brand','Brand'] ] },
    shina: { title: 'Shina', endpoint: '/api/shinas', fields: [ ['name','Name'], ['brand','Brand'] ] },
    bolt: { title: 'Bolts', endpoint: '/api/bolts', fields: [ ['name','Name'], ['brand','Brand'] ] }
  }

  const def = definitions[view]

  async function fetchList() {
    setLoading(true)
    try {
      const res = await fetch(`${API_BASE}${def.endpoint}`)
      if (!res.ok) throw new Error(`HTTP ${res.status}`)
      const json = await res.json()
      setData(json)
      setError(null)
    } catch (err) { console.error('fetchList', err); setData([]) }
    finally { setLoading(false) }
  }

  useEffect(() => { setForm({}); fetchList() }, [view])

  async function handleCreate(e) {
    e?.preventDefault()
    const payload = {}
    def.fields.forEach(f => { const k = f[0]; payload[k] = form[k] ?? '' })
    // coerce numbers for year/price
    if (payload.year) payload.year = parseInt(payload.year)
    if (payload.price) payload.price = parseFloat(payload.price)
    await fetch(`${API_BASE}${def.endpoint}`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) })
    setForm({})
    fetchList()
  }

  async function handleDelete(id) {
    if (!confirm('Delete this item?')) return
    await fetch(`${API_BASE}${def.endpoint}/${id}`, { method: 'DELETE' })
    fetchList()
  }

  return (
    <div id="root">
      <header className="nav">
        <div className="brand">Warehouse</div>
        <nav>
          {Object.keys(definitions).map(k => (
            <button key={k} className={k === view ? 'active' : ''} onClick={() => setView(k)}>{definitions[k].title}</button>
          ))}
        </nav>
      </header>

      <main>
        <section className="panel">
          <h2>{def.title}</h2>

          <form className="small-form" onSubmit={handleCreate}>
            {def.fields.map(f => (
              <input key={f[0]} placeholder={f[1]} value={form[f[0]] ?? ''} onChange={e => setForm({ ...form, [f[0]]: e.target.value })} required />
            ))}
            <button type="submit">Add</button>
            <button type="button" className="ghost" onClick={() => setForm({})}>Reset</button>
          </form>

          <div className="actions">
            <button onClick={fetchList} disabled={loading}>{loading ? 'Loading...' : 'Refresh'}</button>
          </div>

          {data.length === 0 && !loading ? (
            <div className="empty">No items found. Ensure the backend is running at {API_BASE} and check browser console for errors.</div>
          ) : (
            <table className="data-table">
            <thead>
              <tr>
                <th>Id</th>
                {def.fields.map(f => <th key={f[0]}>{f[1]}</th>)}
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {data.map(item => (
                <tr key={item.id}>
                  <td>{item.id}</td>
                  {def.fields.map(f => <td key={f[0]}>{item[f[0]]}</td>)}
                  <td><button className="danger" onClick={() => handleDelete(item.id)}>Delete</button></td>
                </tr>
              ))}
            </tbody>
            </table>
          )}
          {error && <div className="empty">Error: {String(error)}</div>}
        </section>
      </main>
    </div>
  )
}

export default App
