import { Expression } from "../Abstract/Expression";
import { Return, Type } from "../Abstract/Return";
import { Environment } from "../Symbols/Environment";

export class Arithmetic extends Expression {
  constructor(
    private left: Expression,
    private right: Expression,
    private operator: string,
    line: number,
    column: number
  ) {
    super(line, column);
  }

  public execute(environment: Environment) {
    const left = this.left.execute(environment);
    const right = this.right.execute(environment);
    switch (this.operator) {
      case "+":
        return {
          value: left.value + right.value,
          type: this.getType(left, right),
        };
      case "-":
        return {
          value: left.value - right.value,
          type: this.getType(left, right),
        };
      case "*":
        return {
          value: left.value * right.value,
          type: this.getType(left, right),
        };
      case "/":
        return {
          value: left.value / right.value,
          type: this.getType(left, right),
        };
      case "%":
        return {
          value: left.value % right.value,
          type: this.getType(left, right),
        };
      default:
        throw new Error(`Operator ${this.operator} does not exist`);
    }
  }

  private getType(left: Return, right: Return) {
    if (left.type === Type.STRING || right.type === Type.STRING) {
      return Type.STRING;
    }
    return Type.NUMBER;
  }
}
