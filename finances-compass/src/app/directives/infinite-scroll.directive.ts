import { Directive, ElementRef, HostListener, Output, EventEmitter } from '@angular/core';

@Directive({
  selector: '[appInfiniteScroll]'
})
export class InfiniteScrollDirective {
  @Output() scrolled = new EventEmitter<void>();

  constructor(private el: ElementRef) {}

  @HostListener('scroll', ['$event'])
  onScroll(event: Event) {

    const el = event.target as HTMLElement;
    if (el.scrollHeight - el.scrollTop <= el.clientHeight) {
      this.scrolled.emit();
    }
  }
}
