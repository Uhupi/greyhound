import type { ClockState } from '../types/ClockState';

export class ApiService {
    static readonly CLOCK_API_URL = 'http://localhost:5050/time';

    static async fetchFromUrl(url: string = this.CLOCK_API_URL): Promise<ClockState> {
        const res = await fetch(url);
        return res.json();
    }
}
