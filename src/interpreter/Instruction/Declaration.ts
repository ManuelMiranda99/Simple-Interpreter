import { Expression } from "../Abstract/Expression";
import { Instruction } from "../Abstract/Instruction";
import { Environment } from "../Symbols/Environment";

export class Declaration extends Instruction {
  constructor(
    private id: string,
    private value: Expression,
    line: number,
    column: number
  ) {
    super(line, column);
  }

  public execute(environment: Environment) {
    const value = this.value.execute(environment);

    environment.saveVariable(this.id, value.value, value.type);
  }
}
