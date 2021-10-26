import { OnInit, ElementRef, Input, Directive } from '@angular/core';

@Directive({ selector: '[focusMe]' })
export class FocusDirective implements OnInit {

  @Input('focusMe') isFocused: boolean;

  constructor(private hostElement: ElementRef) {
    this.isFocused = false;}

  ngOnInit() {
    if (this.isFocused) {
      this.hostElement.nativeElement.focus();
    }
  }
}
