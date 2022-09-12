import { Expression } from "../Abstract/Expression";
import { Instruction } from "../Abstract/Instruction";
import { Type } from "../Abstract/Return";
import { Environment } from "../Symbols/Environment";

export class While extends Instruction {
  constructor(
    private condition: Expression,
    private instructions: Instruction[],
    line: number,
    column: number
  ) {
    super(line, column);
  }

  public execute(environment: Environment) {
    let condition = this.condition.execute(environment);
    if (condition.type !== Type.BOOLEAN)
      throw new Error(`Condition must be boolean`);

    while (condition.value) {
      for (const instruction of this.instructions) {
        instruction.execute(environment);
      }

      condition = this.condition.execute(environment);
      if (condition.type !== Type.BOOLEAN)
        throw new Error(`Condition must be boolean`);
    }
  }
}
