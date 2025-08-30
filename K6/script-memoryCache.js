import http from 'k6/http';
import { check } from 'k6';
import { htmlReport } from "https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js";
export let options = {
    vus: 50,             // number of virtual users
    duration: '2s',     // how long to run the test
};

export default function () {
    const productId = 1;
    const url = `https://localhost:7207/products-memory-cache/${productId}`; // <-- change to your API base URL

    const res = http.get(url);

    check(res, {
        'status is 200': (r) => r.status === 200,
    });

}

export function handleSummary(data) {
  return {
    "summary-MemoryCache.html": htmlReport(data),
  };
}