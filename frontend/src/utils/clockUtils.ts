export function minutesTopColor(index: number, lit: boolean): string {
  if (!lit) return '#2a2a2a';
  return (index + 1) % 3 === 0 ? '#cc2200' : '#ffd700';
}
